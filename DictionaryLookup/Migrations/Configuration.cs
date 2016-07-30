namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DictionaryLookup.Models;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<DictionaryLookup.Models.DictionaryLookupContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DictionaryLookup.Models.DictionaryLookupContext context)
        {
            // Empties the Dictionary Words table
            context.Database.ExecuteSqlCommand("DELETE FROM WordStrings");
            context.Database.ExecuteSqlCommand("DELETE FROM NGramEntry");
            context.SaveChanges();

            //List<DictionaryWord> wordsToAdd = new List<DictionaryWord>();
            WordString[] wordsArray = new WordString[1000];
            NGramEntry[] nGramArray = new NGramEntry[1000];

            Int64 wordcount = 0;

            using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
            using (TextReader tr = new StreamReader(fs))
            {
                List<string> continueStrings = new List<string>();
                List<Int32> continueCosts = new List<int>();
                List<Int32> continueNGram = new List<int>();
                while (tr.Peek() > -1)
                {
                    string line = tr.ReadLine();

                    // Extract the word
                    string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
                    int NGram = 1;
                    foreach (char c in nGramString) if (c == ' ') NGram++;

                    string tagString = "";
                    if (line.Contains("\t#"))
                    {
                        tagString = line.Substring(line.IndexOf("\t#") + 2);
                    }
                    else if (line.Contains("\t|"))
                    {
                        tagString = line.Substring(line.IndexOf("\t|") + 2);
                    }

                    // Unwrap the continue costs back so that the last one matches the current label
                    while ((continueStrings.Count > 0) && (!line.StartsWith(continueStrings.Last())))
                    {
                        continueStrings.RemoveRange(continueStrings.Count - 1, 1);
                        continueCosts.RemoveRange(continueCosts.Count - 1, 1);
                        continueNGram.RemoveRange(continueNGram.Count - 1, 1);
                    }

                    // Update the continue node state
                    if (tagString.Contains("1="))
                    {
                        Int32 continueCost = Int32.Parse(tagString.Substring(tagString.IndexOf("1=0x") + 10, 2), System.Globalization.NumberStyles.HexNumber);
                        if ((continueStrings.Count > 0) && (continueNGram.Last() == NGram))
                        {
                            continueCost += continueCosts.Last();
                        }
                        continueStrings.Add(nGramString);
                        continueCosts.Add(continueCost);
                        continueNGram.Add(NGram);
                    }
                    if (!line.Contains("\t#"))
                    {
                        // Parse Tag1 for this line
                        Int16 stopCost = (Int16)(0);
                        // Continue cost resets at the space so only add the continue cost if the length of the continue string is at or beyond the
                        // the last space in the ngram we're adding
                        if ((continueStrings.Count > 0) && (continueNGram.Last() == NGram))
                        {
                            stopCost = (Int16)(continueCosts.Last());
                        }
                        Int16 backoffCost = 0;
                        bool badWord = false;
                        if (tagString.Contains("1=0x"))
                        {
                            // Extract the stop cost and add to the current continue cost
                            string tag1ValueString = tagString.Substring(tagString.IndexOf("1=0x") + 4, 8);
                            stopCost += Int16.Parse(tag1ValueString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            // Extract the backoff cost
                            backoffCost = Int16.Parse(tag1ValueString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            // Extract the badword bit
                            badWord = ((Int32.Parse(tag1ValueString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber) & 1) == 1);
                        }

                        Int64 ngtID = ReadOrAddNGramTag(context, new NGramTags(tagString, stopCost, backoffCost, badWord));
                        Int64 ngeID = ReadOrCreateNGramEntry(context, nGramString);


                    }
                }
            }

                            /*
                        //if (wordsArray[wordcount % wordsArray.Length] == null)
                        {
                            wordsArray[wordcount % wordsArray.Length] = new WordString();
                        }
                        wordsArray[wordcount % wordsArray.Length].WordString = wordcount;
                        wordsArray[wordcount % wordsArray.Length].Set(word, spellerRestricted, spellerFrequency, stopCost, backoffCost, badWord, hwrCost, hwrCallig);

                        if (++wordcount % wordsArray.Length == 0)
                        {
                            context.WordStrings.AddOrUpdate(wordsArray);
                            context.WordStrings.AddOrUpdate(wordsArray);
                            context.SaveChanges();
                        }
                    }
                }
            }
            for (int i = 0; i < wordcount % wordsArray.Length; i++)
            {
                context.DictionaryWords.Add(wordsArray[i]);
            }
            context.SaveChanges();
            */
        }


        private Int64 ReadOrAddNGramTag(DictionaryLookup.Models.DictionaryLookupContext context, NGramTags ngt)
        {
            var nid = from a in context.NGramTags
                      where a.TextPredictionCost.Equals(ngt.TextPredictionCost)
                      where a.TextPredictionBackOffCost.Equals(ngt.TextPredictionBackOffCost)
                      where a.HWRCalligraphyCost.Equals(ngt.HWRCalligraphyCost)
                      where a.HWRCost.Equals(ngt.HWRCost)
                      where a.Restricted.Equals(ngt.Restricted)
                      where a.SpellerFrequency.Equals(ngt.SpellerFrequency)
                      where a.TextPredictionBadWord.Equals(ngt.TextPredictionBadWord)
                      select a.NGramTagsID;
            if (nid.Count() > 0)
            {
                return nid.First();
            }
            context.NGramTags.Add(ngt);
            return ngt.NGramTagsID;
        }

        public Int64 ReadOrCreateNGramEntry(DictionaryLookup.Models.DictionaryLookupContext context, string ngram)
        {
            Int64 WordID = 0;
            Int64 Previous1WordID = 0;
            Int64 Previous2WordID = 0;
            string[] words = ngram.Split(' ');
            Int32 NGram = words.Count();

            WordID = ReadOrCreateWord(context, words[NGram - 1]);

            if (NGram > 1)
            {
                Previous1WordID = ReadOrCreateWord(context, words[NGram - 2]);
            }

            if (NGram > 2)
            {
                Previous2WordID = ReadOrCreateWord(context, words[NGram - 3]);
            }

            // Made sure all the words are in the WordStrings table now we need to find or create an NGram entry
            var wid = from a in context.NGramEntries
                      where a.WordID.Equals(WordID)
                      where a.Previous1WordID.Equals(Previous1WordID)
                      where a.Previous2WordID.Equals(Previous2WordID)
                      select a.NGramEntryID;
            if (wid.Count() == 0)
            {
                return wid.First();
            }
            NGramEntry nge = new NGramEntry(WordID, Previous1WordID, Previous2WordID);
            context.NGramEntries.Add(nge);
            return nge.NGramEntryID;
        }

        private Int64 ReadOrCreateWord(DictionaryLookup.Models.DictionaryLookupContext context, string word)
        {
            var wid = from a in context.WordStrings
                      where a.Word.Equals(word)
                      select a.WordStringID;
            if (wid.Count() > 0)
            {
                return wid.First();
            }
            WordString ws = new WordString(word);
            context.WordStrings.Add(ws);
            return ws.WordStringID;
        }
    }
}
