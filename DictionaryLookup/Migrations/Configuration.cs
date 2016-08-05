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
    using System.Threading;

    internal sealed class Configuration : DbMigrationsConfiguration<DictionaryLookup.Models.DictionaryLookupContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DictionaryLookup.Models.DictionaryLookupContext context)
        {
            return;
            // Empties the Dictionary Words table
            //context.Database.ExecuteSqlCommand("DELETE FROM WordStrings");
            //context.Database.ExecuteSqlCommand("DELETE FROM NGramEntry");
            //context.SaveChanges();

            //List<DictionaryWord> wordsToAdd = new List<DictionaryWord>();

            //List<string> allWords = new List<string>();
            //SortedList<string, int> allTheWords = new SortedList<string, int>();

            context.Configuration.AutoDetectChangesEnabled = false;

            //// Parse the file and find all the words
            //if (false)
            //{
            //    HashSet<string> allWords = new HashSet<string>();
            //    HashSet<WordString> allWordStrings = new HashSet<WordString>();
            //    using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
            //    using (TextReader tr = new StreamReader(fs))
            //    {
            //        while (tr.Peek() > -1)
            //        {
            //            string line = tr.ReadLine();
            //            if (!line.Contains("\t#"))
            //            {
            //                // Extract the words
            //                string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
            //                string w1 = "", p1 = "", p2 = "";
            //                int n = WordStringsFromNGramString(nGramString, ref w1, ref p1, ref p2);
            //                if (allWords.Add(w1))
            //                {
            //                    allWordStrings.Add(new WordString(w1));
            //                }
            //                if (n > 1 && allWords.Add(p1))
            //                {
            //                    allWordStrings.Add(new WordString(p1));
            //                }
            //                if (n > 2 && allWords.Add(p2))
            //                {
            //                    allWordStrings.Add(new WordString(p2));
            //                }
            //            }
            //        }
            //    }
            //    context.WordStrings.AddRange(allWordStrings);
            //    context.SaveChanges();
            //    return;
            //}


            //// Re-parse the file to find all the NGram Tags
            //if (false)
            //{
            //    HashSet<Int64> allTagsHash = new HashSet<Int64>();
            //    HashSet<NGramTags> allNGramTags = new HashSet<NGramTags>();
            //    using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
            //    using (TextReader tr = new StreamReader(fs))
            //    {
            //        intermediateStates.Clear();
            //        while (tr.Peek() > -1)
            //        {
            //            NGramTags ngt = ParseTags(tr.ReadLine());
            //            if (ngt != null)
            //            {
            //                Int64 ngh = ngt.GetHash();
            //                if (allTagsHash.Add(ngh))
            //                {
            //                    allNGramTags.Add(ngt);
            //                }
            //            }
            //        }
            //    }
            //    context.NGramTags.AddRange(allNGramTags);
            //    context.SaveChanges();
            //    return;
            //}

            //// Re-parse to find the NGrams
            //if (false)
            //{

            //    List<NGramEntry> allNGrams = new List<NGramEntry>();
            //    using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
            //    using (TextReader tr = new StreamReader(fs))
            //    {
            //        // Create a hash table of all the words 
            //        Dictionary<string, Int64> wordStringTable = new Dictionary<string, long>();
            //        {
            //            var wss = from a in context.WordStrings select a;
            //            foreach (WordString ws in wss)
            //            {
            //                wordStringTable.Add(ws.Word, ws.WordStringID);
            //            }
            //        }
            //        while (tr.Peek() > -1)
            //        {
            //            string line = tr.ReadLine();
            //            if (!line.Contains("\t#"))
            //            {
            //                // Extract the words
            //                string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
            //                string w1 = "", p1 = "", p2 = "";
            //                int n = WordStringsFromNGramString(nGramString, ref w1, ref p1, ref p2);
            //                allNGrams.Add(new NGramEntry(wordStringTable[w1],
            //                                             (n > 1) ? wordStringTable[p1] : 0,
            //                                             (n > 2) ? wordStringTable[p2] : 0));
            //            }
            //        }
            //    }
            //    for (int c = 0; c < allNGrams.Count(); c += 100000)
            //    {
            //        int n = Math.Min(100000, allNGrams.Count() - c);
            //        context.NGramEntries.AddRange(allNGrams.GetRange(c, n));
            //        context.SaveChanges();
            //    }
            //    return;
            //}

            //if (true)
            //{
            //    {
            //        List<Models.DictionaryEntry> dictionaryEntryTable = new List<Models.DictionaryEntry>();

            //        CreateDET(ref dictionaryEntryTable, context);


            //        using (FileStream fs = File.OpenWrite(@"C:\temp\dictionaryEntryTable.txt"))
            //        using (TextWriter tw = new StreamWriter(fs))
            //        {
            //            foreach (Models.DictionaryEntry de in dictionaryEntryTable)
            //            {
            //                tw.WriteLine(de.NGramEntryID.ToString() + " " + de.NGramTagID.ToString());
            //            }
            //        }
            //        dictionaryEntryTable.Clear();
            //        dictionaryEntryTable.TrimExcess();

            //    }
            //    GC.Collect();
            //    Thread.Sleep(10000);
            //    /*
            //    while (dictionaryEntryTable.Count() > 0)
            //    {
            //        List<Models.DictionaryEntry> copydict = dictionaryEntryTable.GetRange(0, Math.Min(1000, dictionaryEntryTable.Count()));
            //        context.DictionaryEntries.AddRange(copydict);
            //        context.SaveChanges();
            //        dictionaryEntryTable.RemoveRange(0, copydict.Count());
            //        dictionaryEntryTable.TrimExcess();
            //    }
            //    */
            //}
        }

    //    private void CreateDET(ref List<DictionaryLookup.Models.DictionaryEntry> dictionaryEntryTable, DictionaryLookup.Models.DictionaryLookupContext context)
    //    {
    //        // Create a hash table of all the NGram tags
    //        Dictionary<Int64, Int64> nGramTagTable = new Dictionary<Int64, Int64>(); // Hash of the tags as key, table index as the value
    //        {
    //            var ngts = from a in context.NGramTags
    //                       select a;
    //            foreach (NGramTags ngt in ngts)
    //            {
    //                nGramTagTable.Add(ngt.GetHash(), ngt.NGramTagsID);
    //            }
    //        }
    //        // Create a hash of all the words
    //        Dictionary<Int64, string> wordIndexTable = new Dictionary<Int64, string>();
    //        {
    //            var wss = from a in context.WordStrings select a;
    //            foreach (WordString ws in wss)
    //            {
    //                wordIndexTable.Add(ws.WordStringID, ws.Word);
    //            }
    //        }

    //        // Hash of NGram strings to NGram tag index
    //        // Then we can look up the strings in this hash and get the associated NGram string index
    //        Dictionary<string, Int64> nGramEntryTable = new Dictionary<string, Int64>(); // Hash of the strings as key, table index as the value
    //        var nges = from a in context.NGramEntries select a;
    //        foreach (NGramEntry nge in nges)
    //        {
    //            string ngs = ((nge.Previous2WordID > 0) ? (wordIndexTable[nge.Previous2WordID] + " ") : "") +
    //                         ((nge.Previous1WordID > 0) ? (wordIndexTable[nge.Previous1WordID] + " ") : "") +
    //                         wordIndexTable[nge.WordID];
    //            nGramEntryTable.Add(ngs, nge.NGramEntryID);
    //        }

    //        // We have the fully qualifed NGram strings so we don't need the individual words any more
    //        wordIndexTable.Clear();

    //        // Now we fill in the final table with the indexes to the tags and ngram string tables
    //        using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
    //        using (TextReader tr = new StreamReader(fs))
    //        {
    //            while (tr.Peek() > -1)
    //            {
    //                string line = tr.ReadLine();
    //                NGramTags ngt = ParseTags(line);
    //                if (ngt != null)
    //                {
    //                    string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
    //                    if (dictionaryEntryTable.Count() < 50000)
    //                    {
    //                        dictionaryEntryTable.Add(new Models.DictionaryEntry(nGramEntryTable[nGramString], nGramTagTable[ngt.GetHash()], 0));
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    private int WordStringsFromNGramString(string nGramString, ref string w1, ref string p1, ref string p2)
    //    {
    //        p1 = ""; p2 = "";
    //        string[] words = nGramString.Split(' ');
    //        int wordCount = words.Count();
    //        w1 = words[wordCount - 1];
    //        if(wordCount > 1)
    //        {
    //            p1 = words[wordCount - 2];
    //        }
    //        if(wordCount > 2)
    //        {
    //            p2 = words[wordCount - 3];
    //            // Prepend all the other words to the p2 in the case of quad-grams and above
    //            for (int i = wordCount - 4; i >= 0; i--)
    //            {
    //                p2 = words[i] + " " + p2;
    //            }
    //            return 3;
    //        }
    //        return wordCount;
    //    }

    //    private NGramTags ParseTags(string line)
    //    {
    //        string nGramString = line.Split('\t')[0];
    //        int NGram = nGramString.Split(' ').Count();
    //        string tagString = line.Contains("\t#") ? line.Substring(line.IndexOf("\t#") + 2) :
    //                           line.Contains("\t|") ? line.Substring(line.IndexOf("\t|") + 2) : "";

    //        // Unwrap the continue costs back so that the last one matches the current label
    //        while ((intermediateStates.Count > 0) && (!line.StartsWith(intermediateStates.Last().NGramString)))
    //        {
    //            intermediateStates.RemoveAt(intermediateStates.Count - 1);
    //        }

    //        // Update the continue node state
    //        if (tagString.Contains("1=0x"))
    //        {
    //            Int32 continueCost = Int32.Parse(tagString.Substring(tagString.IndexOf("1=0x") + 10, 2), System.Globalization.NumberStyles.HexNumber);
    //            if ((intermediateStates.Count > 0) && (intermediateStates.Last().NGramLength == NGram))
    //            {
    //                continueCost += intermediateStates.Last().Cost;
    //            }
    //            intermediateStates.Add(new IntermediateTags(nGramString, (Int16)continueCost, (Int16)NGram));
    //        }
    //        if (!line.Contains("\t#"))
    //        {
    //            // Parse Tag1 for this line
    //            // Continue cost resets at the space so only add the continue cost if the length of the continue string is at or beyond the
    //            // the last space in the ngram we're adding
    //            Int16 stopCost = (Int16)(((intermediateStates.Count > 0) && (intermediateStates.Last().NGramLength == NGram)) ? (intermediateStates.Last().Cost) : 0);
    //            Int16 backoffCost = 0;
    //            bool badWord = false;
    //            if (tagString.Contains("1=0x"))
    //            {
    //                // Extract the stop cost and add to the current continue cost
    //                string tag1ValueString = tagString.Substring(tagString.IndexOf("1=0x") + 4, 8);
    //                stopCost += Int16.Parse(tag1ValueString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
    //                // Extract the backoff cost
    //                backoffCost = Int16.Parse(tag1ValueString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
    //                // Extract the badword bit
    //                badWord = ((Int32.Parse(tag1ValueString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber) & 1) == 1);
    //            }
    //            NGramTags ngt = new NGramTags(tagString, stopCost, backoffCost, badWord);
    //            return ngt;
    //        }
    //        return null;
    //    }

    //    struct IntermediateTags
    //    {
    //        public IntermediateTags(string s, Int16 c, Int16 n) { NGramLength = n; Cost = c; NGramString = s; }
    //        public string NGramString;
    //        public Int16 Cost;
    //        public Int16 NGramLength;
    //    }

    //    private List<IntermediateTags> intermediateStates = new List<IntermediateTags>();






    //    /*
    //    Int64 ngramcount = 0;

    //    using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\english_united_states.testtrie.txt"))
    //    using (TextReader tr = new StreamReader(fs))
    //    {
    //        List<string> continueStrings = new List<string>();
    //        List<Int32> continueCosts = new List<int>();
    //        List<Int32> continueNGram = new List<int>();
    //        while (tr.Peek() > -1)
    //        {
    //            string line = tr.ReadLine();

    //            // Extract the word
    //            string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
    //            int NGram = 1;
    //            foreach (char c in nGramString) if (c == ' ') NGram++;

    //            string tagString = "";
    //            if (line.Contains("\t#"))
    //            {
    //                tagString = line.Substring(line.IndexOf("\t#") + 2);
    //            }
    //            else if (line.Contains("\t|"))
    //            {
    //                tagString = line.Substring(line.IndexOf("\t|") + 2);
    //            }

    //            // Unwrap the continue costs back so that the last one matches the current label
    //            while ((continueStrings.Count > 0) && (!line.StartsWith(continueStrings.Last())))
    //            {
    //                continueStrings.RemoveRange(continueStrings.Count - 1, 1);
    //                continueCosts.RemoveRange(continueCosts.Count - 1, 1);
    //                continueNGram.RemoveRange(continueNGram.Count - 1, 1);
    //            }

    //            // Update the continue node state
    //            if (tagString.Contains("1="))
    //            {
    //                Int32 continueCost = Int32.Parse(tagString.Substring(tagString.IndexOf("1=0x") + 10, 2), System.Globalization.NumberStyles.HexNumber);
    //                if ((continueStrings.Count > 0) && (continueNGram.Last() == NGram))
    //                {
    //                    continueCost += continueCosts.Last();
    //                }
    //                continueStrings.Add(nGramString);
    //                continueCosts.Add(continueCost);
    //                continueNGram.Add(NGram);
    //            }
    //            if (!line.Contains("\t#"))
    //            {
    //                // Parse Tag1 for this line
    //                Int16 stopCost = (Int16)(0);
    //                // Continue cost resets at the space so only add the continue cost if the length of the continue string is at or beyond the
    //                // the last space in the ngram we're adding
    //                if ((continueStrings.Count > 0) && (continueNGram.Last() == NGram))
    //                {
    //                    stopCost = (Int16)(continueCosts.Last());
    //                }
    //                Int16 backoffCost = 0;
    //                bool badWord = false;
    //                if (tagString.Contains("1=0x"))
    //                {
    //                    // Extract the stop cost and add to the current continue cost
    //                    string tag1ValueString = tagString.Substring(tagString.IndexOf("1=0x") + 4, 8);
    //                    stopCost += Int16.Parse(tag1ValueString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
    //                    // Extract the backoff cost
    //                    backoffCost = Int16.Parse(tag1ValueString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
    //                    // Extract the badword bit
    //                    badWord = ((Int32.Parse(tag1ValueString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber) & 1) == 1);
    //                }

    //                Int64 ngtID = ReadOrAddNGramTag(context, new NGramTags(tagString, stopCost, backoffCost, badWord));
    //                Int64 ngeID = ReadOrCreateNGramEntry(context, nGramString);

    //                Models.DictionaryEntry de = new Models.DictionaryEntry(ngeID, ngtID);

    //                context.DictionaryEntries.Add(de);
    //                context.SaveChanges();

    //                //if(ngramcount++ >= 1000)
    //                //{
    //                //    return;
    //                //}

    //            }
    //        }
    //    }
    //}
    //    */ 


    //    private Int64 ReadOrAddNGramTag(DictionaryLookup.Models.DictionaryLookupContext context, NGramTags ngt)
    //    {
    //        var nid = from a in context.NGramTags
    //                  where a.TextPredictionCost == ngt.TextPredictionCost
    //                  where a.TextPredictionBackOffCost == ngt.TextPredictionBackOffCost
    //                  where a.HWRCalligraphyCost == ngt.HWRCalligraphyCost
    //                  where a.HWRCost == ngt.HWRCost
    //                  where a.Restricted == ngt.Restricted
    //                  where a.SpellerFrequency == ngt.SpellerFrequency
    //                  where a.TextPredictionBadWord == ngt.TextPredictionBadWord
    //                  select a.NGramTagsID;
    //        if (nid.Count() > 0)
    //        {
    //            return nid.First();
    //        }
    //        context.NGramTags.Add(ngt);
    //        return ngt.NGramTagsID;
    //    }

    //    public Int64 ReadOrCreateNGramEntry(DictionaryLookup.Models.DictionaryLookupContext context, string ngram)
    //    {
    //        Int64 WordID = 0;
    //        Int64 Previous1WordID = 0;
    //        Int64 Previous2WordID = 0;
    //        string[] words = ngram.Split(' ');
    //        Int32 NGram = words.Count();

    //        WordID = ReadOrCreateWord(context, words[NGram - 1]);

    //        // In the rare case where a word==previousword (e.g. "very very") and was just created and added to the DB
    //        // the query will fail to find it until the recent add is saved. So we need to compare the words first
    //        // We only save after each dictionary entry.
    //        if (NGram > 1)
    //        {
    //            Previous1WordID = (words[NGram - 2] == words[NGram - 1]) ? WordID : ReadOrCreateWord(context, words[NGram - 2]);
    //        }

    //        if (NGram > 2)
    //        {
    //            Previous2WordID = (words[NGram - 3] == words[NGram - 1]) ? WordID :
    //                (words[NGram - 3] == words[NGram - 2]) ? Previous1WordID : ReadOrCreateWord(context, words[NGram - 3]);
    //        }

    //        // Made sure all the words are in the WordStrings table now we need to find or create an NGram entry
    //        var wid = from a in context.NGramEntries
    //                  where a.WordID == WordID
    //                  where a.Previous1WordID == Previous1WordID
    //                  where a.Previous2WordID == Previous2WordID
    //                  select a.NGramEntryID;
    //        if (wid.Count() > 0)
    //        {
    //            return wid.First();
    //        }
    //        NGramEntry nge = new NGramEntry(WordID, Previous1WordID, Previous2WordID);
    //        context.NGramEntries.Add(nge);
    //        return nge.NGramEntryID;
    //    }

    //    private Int64 ReadOrCreateWord(DictionaryLookup.Models.DictionaryLookupContext context, string word)
    //    {
    //        var wid = from a in context.WordStrings
    //                  where a.Word == word
    //                  select a.WordStringID;
    //        if (wid.Count() > 0)
    //        {
    //            return wid.First();
    //        }
    //        WordString ws = new WordString(word);
    //        context.WordStrings.Add(ws);
    //        return ws.WordStringID;
    //    }
    }
}
