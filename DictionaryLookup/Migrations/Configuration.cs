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
            //context.Database.ExecuteSqlCommand("DELETE FROM WordStrings");
            //context.Database.ExecuteSqlCommand("DELETE FROM NGramEntry");
            //context.SaveChanges();

            //List<DictionaryWord> wordsToAdd = new List<DictionaryWord>();

            //List<string> allWords = new List<string>();
            //SortedList<string, int> allTheWords = new SortedList<string, int>();


            // Parse the file and find all the words
            if (false)
            {
                HashSet<string> allWords = new HashSet<string>();
                HashSet<WordString> allWordStrings = new HashSet<WordString>();
                using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
                using (TextReader tr = new StreamReader(fs))
                {
                    while (tr.Peek() > -1)
                    {
                        string line = tr.ReadLine();
                        if (!line.Contains("\t#"))
                        {
                            // Extract the words
                            string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
                            foreach (string w in nGramString.Split(' '))
                            {
                                if (allWords.Add(w))
                                {
                                    allWordStrings.Add(new WordString(w));
                                }
                            }
                        }
                    }
                }
                context.WordStrings.AddRange(allWordStrings);
                context.SaveChanges();
            }

            // Re-parse the file to find all the NGram Tags
            if (false)
            {
                HashSet<Int64> allTagsHash = new HashSet<Int64>();
                HashSet<NGramTags> allNGramTags = new HashSet<NGramTags>();
                using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
                using (TextReader tr = new StreamReader(fs))
                {
                    intermediateStates.Clear();
                    while (tr.Peek() > -1)
                    {
                        NGramTags ngt = ParseTags(tr.ReadLine());
                        if (ngt != null)
                        {
                            Int64 ngh = ngt.GetHash();
                            if (allTagsHash.Add(ngh))
                            {
                                allNGramTags.Add(ngt);
                            }
                        }
                    }
                }
                context.NGramTags.AddRange(allNGramTags);
                context.SaveChanges();
            }

            // Re-parse to find the NGrams
            if (true)
            {
                // Create a hash table of all the words 
                Dictionary<string, Int64> wordStringTable = new Dictionary<string, long>();
                {
                    var wss = from a in context.WordStrings
                              select a;
                    foreach (WordString ws in wss)
                    {
                        wordStringTable.Add(ws.Word, ws.WordStringID);
                    }
                }
                List<NGramEntry> allNGrams = new List<NGramEntry>();
                using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
                using (TextReader tr = new StreamReader(fs))
                {
                    while (tr.Peek() > -1)
                    {
                        string line = tr.ReadLine();
                        if (!line.Contains("\t#"))
                        {
                            // Extract the words
                            string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
                            string[] words = nGramString.Split(' ');
                            Int64 wid = wordStringTable[words.Last().ToString()];
                            Int64 p1id = (words.Count() > 1) ? wordStringTable[words[words.Count() - 1].ToString()] : 0;
                            Int64 p2id = (words.Count() > 2) ? wordStringTable[words[words.Count() - 2].ToString()] : 0;
                            allNGrams.Add(new NGramEntry(wid, p1id, p2id));
                        }
                    }
                }
                //File.WriteAllText(@"C:\temp\log.txt", allNGrams.Count().ToString());
                for (int c = 0; c < allNGrams.Count(); c += 100000)
                {
                    int n = Math.Min(100000, allNGrams.Count() - c);
                    context.NGramEntries.AddRange(allNGrams.GetRange(c, n));
                    context.SaveChanges();
                }
            }

            /*
            */
        }

        private NGramTags ParseTags(string line)
        {
            string nGramString = line.Split('\t')[0];
            int NGram = nGramString.Split(' ').Count();
            string tagString = line.Contains("\t#") ? line.Substring(line.IndexOf("\t#") + 2) :
                               line.Contains("\t|") ? line.Substring(line.IndexOf("\t|") + 2) : "";

            // Unwrap the continue costs back so that the last one matches the current label
            while ((intermediateStates.Count > 0) && (!line.StartsWith(intermediateStates.Last().NGramString)))
            {
                intermediateStates.RemoveAt(intermediateStates.Count - 1);
            }

            // Update the continue node state
            if (tagString.Contains("1=0x"))
            {
                Int32 continueCost = Int32.Parse(tagString.Substring(tagString.IndexOf("1=0x") + 10, 2), System.Globalization.NumberStyles.HexNumber);
                if ((intermediateStates.Count > 0) && (intermediateStates.Last().NGramLength == NGram))
                {
                    continueCost += intermediateStates.Last().Cost;
                }
                intermediateStates.Add(new IntermediateTags(nGramString, (Int16)continueCost, (Int16)NGram));
            }
            if (!line.Contains("\t#"))
            {
                // Parse Tag1 for this line
                // Continue cost resets at the space so only add the continue cost if the length of the continue string is at or beyond the
                // the last space in the ngram we're adding
                Int16 stopCost = (Int16)(((intermediateStates.Count > 0) && (intermediateStates.Last().NGramLength == NGram)) ? (intermediateStates.Last().Cost) : 0);
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
                NGramTags ngt = new NGramTags(tagString, stopCost, backoffCost, badWord);
                return ngt;
            }
            return null;
        }

        struct IntermediateTags
        {
            public IntermediateTags(string s, Int16 c, Int16 n) { NGramLength = n; Cost = c; NGramString = s; }
            public string NGramString;
            public Int16 Cost;
            public Int16 NGramLength;
        }

        private List<IntermediateTags> intermediateStates = new List<IntermediateTags>();






        /*
        Int64 ngramcount = 0;

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

                    Models.DictionaryEntry de = new Models.DictionaryEntry(ngeID, ngtID);

                    context.DictionaryEntries.Add(de);
                    context.SaveChanges();

                    //if(ngramcount++ >= 1000)
                    //{
                    //    return;
                    //}

                }
            }
        }
    }
        */ 


        private Int64 ReadOrAddNGramTag(DictionaryLookup.Models.DictionaryLookupContext context, NGramTags ngt)
        {
            var nid = from a in context.NGramTags
                      where a.TextPredictionCost == ngt.TextPredictionCost
                      where a.TextPredictionBackOffCost == ngt.TextPredictionBackOffCost
                      where a.HWRCalligraphyCost == ngt.HWRCalligraphyCost
                      where a.HWRCost == ngt.HWRCost
                      where a.Restricted == ngt.Restricted
                      where a.SpellerFrequency == ngt.SpellerFrequency
                      where a.TextPredictionBadWord == ngt.TextPredictionBadWord
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

            // In the rare case where a word==previousword (e.g. "very very") and was just created and added to the DB
            // the query will fail to find it until the recent add is saved. So we need to compare the words first
            // We only save after each dictionary entry.
            if (NGram > 1)
            {
                Previous1WordID = (words[NGram - 2] == words[NGram - 1]) ? WordID : ReadOrCreateWord(context, words[NGram - 2]);
            }

            if (NGram > 2)
            {
                Previous2WordID = (words[NGram - 3] == words[NGram - 1]) ? WordID :
                    (words[NGram - 3] == words[NGram - 2]) ? Previous1WordID : ReadOrCreateWord(context, words[NGram - 3]);
            }

            // Made sure all the words are in the WordStrings table now we need to find or create an NGram entry
            var wid = from a in context.NGramEntries
                      where a.WordID == WordID
                      where a.Previous1WordID == Previous1WordID
                      where a.Previous2WordID == Previous2WordID
                      select a.NGramEntryID;
            if (wid.Count() > 0)
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
                      where a.Word == word
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
