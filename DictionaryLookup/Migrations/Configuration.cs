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
            context.ErrorTypes.AddOrUpdate(p => p.ErrorTypeID,
                new ErrorType { ErrorTypeID = 0, ErrorTypeName = "Misspelling" },
                new ErrorType { ErrorTypeID = 1, ErrorTypeName = "Invalid" },
                new ErrorType { ErrorTypeID = 2, ErrorTypeName = "Offensive" },
                new ErrorType { ErrorTypeID = 3, ErrorTypeName = "Inoffensive" });

            context.Users.AddOrUpdate(p => p.UserID,
                new User { UserID = 0, UserContact = "Unknown" },
                new User { UserID = 1, UserContact = "davestev" });

            context.Languages.AddOrUpdate(p => p.LanguagesID,
                new Languages { LanguagesID = 0, BCP47 = "en-US", FriendlyName = "English (United States)" },
                new Languages { LanguagesID = 0, BCP47 = "en-GB", FriendlyName = "English (Great Britain)" },
                new Languages { LanguagesID = 0, BCP47 = "fr-FR", FriendlyName = "French (France)" },
                new Languages { LanguagesID = 0, BCP47 = "it-IT", FriendlyName = "Italian" },
                new Languages { LanguagesID = 0, BCP47 = "de-DE", FriendlyName = "German (Germany)" },
                new Languages { LanguagesID = 0, BCP47 = "es-ES", FriendlyName = "Spanish (Spain)" },
                new Languages { LanguagesID = 0, BCP47 = "es-MX", FriendlyName = "Spanish (Mexico)" });


            /*
            // Empties the Dictionary Words table
            //context.Database.ExecuteSqlCommand("DELETE FROM DictionaryWords");
            //context.SaveChanges();
            
            //List<DictionaryWord> wordsToAdd = new List<DictionaryWord>();
            DictionaryWord[] wordsArray = new DictionaryWord[1000];

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
                    string word = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
                    int NGram = 1;
                    foreach (char c in word) if (c == ' ') NGram++;

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
                        continueStrings.Add(word);
                        continueCosts.Add(continueCost);
                        continueNGram.Add(NGram);
                    }
                    if (!line.Contains("\t#"))
                    {
                        // Parse Tag0 for this word
                        // Dialect filter
                        // US English only for now (i.e. bit 1)
                        bool spellerRestricted = false;
                        Int16 spellerFrequency = 0;
                        if (tagString.Contains("0=0x"))
                        {
                            Int32 dialectTag = Int32.Parse(tagString.Substring(tagString.IndexOf("0=0x") + 11, 1), System.Globalization.NumberStyles.HexNumber);
                            if ((dialectTag & 1) == 0)
                            {
                                // Has a dialect tag and it does not have bit 1 set.
                                continue;
                            }
                            spellerRestricted = ((Int32.Parse(tagString.Substring(tagString.IndexOf("0=0x") + 10, 1), System.Globalization.NumberStyles.HexNumber) & 1) > 0);
                            spellerFrequency = (Int16)(Int32.Parse(tagString.Substring(tagString.IndexOf("0=0x") + 9, 1), System.Globalization.NumberStyles.HexNumber) & 3);
                        }

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
                            string tag1ValueString = tagString.Substring(tagString.IndexOf("1=0x")+4, 8);
                            stopCost += Int16.Parse(tag1ValueString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            // Extract the backoff cost
                            backoffCost = Int16.Parse(tag1ValueString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                            // Extract the badword bit
                            badWord = ((Int32.Parse(tag1ValueString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber) & 1) == 1);
                        }

                        // Parse Tag5
                        Int16 hwrCost = 4091; // Default HWR cost for a valid word
                        Int16 hwrCallig = 0;
                        if (tagString.Contains("5=0x"))
                        {
                            string tag5ValueString = tagString.Substring(tagString.IndexOf("5=0x")+4, 8);
                            hwrCost = Int16.Parse(tag5ValueString.Substring(4, 4), System.Globalization.NumberStyles.HexNumber);
                            hwrCallig = (Int16)(Int32.Parse(tag5ValueString.Substring(5, 1), System.Globalization.NumberStyles.HexNumber) & 3);
                        }

                        //if (wordsArray[wordcount % wordsArray.Length] == null)
                        {
                            wordsArray[wordcount % wordsArray.Length] = new DictionaryWord();
                        }
                        wordsArray[wordcount % wordsArray.Length].DictionaryWordID = wordcount;
                        wordsArray[wordcount % wordsArray.Length].Set(word, spellerRestricted, spellerFrequency, stopCost, backoffCost, badWord, hwrCost, hwrCallig);

                        if (++wordcount % wordsArray.Length == 0)
                        {
                            context.DictionaryWords.AddRange(wordsArray);
                            context.SaveChanges();
                        }
                    }
                }
            }
            for(int i=0; i < wordcount % wordsArray.Length; i++)
            {
                context.DictionaryWords.Add(wordsArray[i]);
            }
            context.SaveChanges();
            */
        }
    }
}
