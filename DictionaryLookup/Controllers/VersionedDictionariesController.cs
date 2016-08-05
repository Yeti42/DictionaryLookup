using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DictionaryLookup.Models;
using System.IO;
using System.Collections;
using System.Threading;

namespace DictionaryLookup.Controllers
{
    public class VersionedDictionariesController : Controller
    {
        private DictionaryLookupContext db = new DictionaryLookupContext();

        // GET: VersionedDictionaries
        public ActionResult Index()
        {
            Upload(AppDomain.CurrentDomain.BaseDirectory + @"\fake.testtrie.txt");
            return View(db.VersionedDictionaries.ToList());
        }

        public void Upload(string filename)
        {
            inputFilename = AppDomain.CurrentDomain.BaseDirectory + @"\fake.testtrie.txt";
            VersionedDictionary vd = new VersionedDictionary(1, "Fake Dictionary");
            db.VersionedDictionaries.Add(vd);
            db.SaveChanges();
            Seed(vd.VersionedDictionaryID);
        }

        private void Seed(int versionIndex)
        {
            /*
            {
                // Need to add these words to the dictionary first before we can
                // get the indexes we require for the ngrams
                HashSet<string> allWords = new HashSet<string>();
                List<WordString> allWordStrings = new List<WordString>();
                {
                    var wss = from a in db.WordStrings select a;
                    foreach (WordString ws in wss)
                    {
                        allWords.Add(ws.Word);
                    }
                }
                // Also Read the NGram tags and add in any new ones that do't already exist.
                HashSet<Int64> allTagHashes = new HashSet<Int64>();
                List<NGramTags> allNGramTags = new List<NGramTags>();
                var ngts = from a in db.NGramTags select a;
                foreach (NGramTags ngt in ngts)
                {
                    allTagHashes.Add(ngt.GetHash());
                }
                TestTrieReader ttr = new TestTrieReader(inputFilename);
                while (ttr.Next())
                {
                    for(int i=0;i<ttr.NGram;i++)
                    {
                        if (allWords.Add(ttr.w[i]))
                        {
                            allWordStrings.Add(new WordString(ttr.w[i]));
                        }
                    }
                    if (allTagHashes.Add(ttr.nGramTags.GetHash()))
                    {
                        allNGramTags.Add(new NGramTags(ttr.nGramTags.GetHash()));
                    }
                }
                if (allWordStrings.Count() > 0)
                {
                    db.WordStrings.AddRange(allWordStrings);
                    db.SaveChanges();
                }
                if (allNGramTags.Count() > 0)
                {
                    db.NGramTags.AddRange(allNGramTags);
                    db.SaveChanges();
                }
            }

            {
                // Copy the words and tags into key/value pairs so that we can very quickly look them up and
                // populate the NGramEntries
                Dictionary<string, Int64> allWordIndexes = new Dictionary<string, Int64>();
                // Add a blank entry for all the unigrams and bigrams
                allWordIndexes.Add("", 0);
                {
                    var wss = from a in db.WordStrings select a;
                    foreach (WordString ws in wss)
                    {
                        allWordIndexes.Add(ws.Word, ws.WordStringID);
                    }
                }
                Dictionary<Int64, Int64> allNGramTagIndexes = new Dictionary<Int64, Int64>();
                {
                    var wss = from a in db.WordStrings select a;
                    foreach (WordString ws in wss)
                    {
                        allWordIndexes.Add(ws.Word, ws.WordStringID);
                    }
                }

                TestTrieReader ttr = new TestTrieReader(inputFilename);
                while (ttr.Next())
                {
                    NGramEntry nge = new NGramEntry(allWordIndexes[ttr.w[0]], allWordIndexes[ttr.w[1]], allWordIndexes[ttr.w[2]], allNGramTagIndexes[ttr.nGramTags.GetHash()], versionIndex);
                }
            }
            */

            /*
            HashSet<string> allWords = new HashSet<string>();
            HashSet<WordString> allWordStrings = new HashSet<WordString>();
            using (FileStream fs = System.IO.File.OpenRead(inputFilename))
            using (TextReader tr = new StreamReader(fs))
            {
                while (tr.Peek() > -1)
                {
                    string line = tr.ReadLine();
                    if (!line.Contains("\t#"))
                    {
                        // Extract the words
                        string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
                        string w1 = "", p1 = "", p2 = "";
                        int n = WordStringsFromNGramString(nGramString, ref w1, ref p1, ref p2);
                        if (allWords.Add(w1))
                        {
                            allWordStrings.Add(new WordString(w1));
                        }
                        if (n > 1 && allWords.Add(p1))
                        {
                            allWordStrings.Add(new WordString(p1));
                        }
                        if (n > 2 && allWords.Add(p2))
                        {
                            allWordStrings.Add(new WordString(p2));
                        }
                    }
                }
            }
            db.WordStrings.AddRange(allWordStrings);
            db.SaveChanges();
            return;

            // Re-parse the file to find all the NGram Tags
            if (false)
            {
                HashSet<Int64> allTagsHash = new HashSet<Int64>();
                HashSet<NGramTags> allNGramTags = new HashSet<NGramTags>();
                using (FileStream fs = System.IO.File.OpenRead(inputFilename))
                using (TextReader tr = new StreamReader(fs))
                {
                    intermediateStates.Clear();
                    while (tr.Peek() > -1)
                    {
                        NGramTags ngt = ParseTags(tr.ReadLine(), ref intermediateStates);
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
                db.NGramTags.AddRange(allNGramTags);
                db.SaveChanges();
                return;
            }
            */

            // Re-parse to find the NGrams
            //if (false)
            //{

            //    List<NGramEntry> allNGrams = new List<NGramEntry>();
            //    using (FileStream fs = System.IO.File.OpenRead(inputFilename))
            //    using (TextReader tr = new StreamReader(fs))
            //    {
            //        // Create a hash table of all the words 
            //        Dictionary<string, Int64> wordStringTable = new Dictionary<string, long>();
            //        {
            //            var wss = from a in db.WordStrings select a;
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
            //        db.NGramEntries.AddRange(allNGrams.GetRange(c, n));
            //        db.SaveChanges();
            //    }
            //    return;
            //}

            //if (true)
            //{
            //    {
            //        List<Models.DictionaryEntry> dictionaryEntryTable = new List<Models.DictionaryEntry>();

            //        CreateDET(ref dictionaryEntryTable, db);


            //        using (FileStream fs = System.IO.File.OpenWrite(@"C:\temp\dictionaryEntryTable.txt"))
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

        //private void CreateDET(ref List<DictionaryLookup.Models.DictionaryEntry> dictionaryEntryTable, DictionaryLookup.Models.DictionaryLookupContext context)
        //{
        //    // Create a hash table of all the NGram tags
        //    Dictionary<Int64, Int64> nGramTagTable = new Dictionary<Int64, Int64>(); // Hash of the tags as key, table index as the value
        //    {
        //        var ngts = from a in context.NGramTags
        //                   select a;
        //        foreach (NGramTags ngt in ngts)
        //        {
        //            nGramTagTable.Add(ngt.GetHash(), ngt.NGramTagsID);
        //        }
        //    }
        //    // Create a hash of all the words
        //    Dictionary<Int64, string> wordIndexTable = new Dictionary<Int64, string>();
        //    {
        //        var wss = from a in context.WordStrings select a;
        //        foreach (WordString ws in wss)
        //        {
        //            wordIndexTable.Add(ws.WordStringID, ws.Word);
        //        }
        //    }

        //    // Hash of NGram strings to NGram tag index
        //    // Then we can look up the strings in this hash and get the associated NGram string index
        //    Dictionary<string, Int64> nGramEntryTable = new Dictionary<string, Int64>(); // Hash of the strings as key, table index as the value
        //    var nges = from a in context.NGramEntries select a;
        //    foreach (NGramEntry nge in nges)
        //    {
        //        string ngs = ((nge.Previous2WordID > 0) ? (wordIndexTable[nge.Previous2WordID] + " ") : "") +
        //                     ((nge.Previous1WordID > 0) ? (wordIndexTable[nge.Previous1WordID] + " ") : "") +
        //                     wordIndexTable[nge.WordID];
        //        nGramEntryTable.Add(ngs, nge.NGramEntryID);
        //    }

        //    // We have the fully qualifed NGram strings so we don't need the individual words any more
        //    wordIndexTable.Clear();

        //    // Now we fill in the final table with the indexes to the tags and ngram string tables
        //    using (FileStream fs = System.IO.File.OpenRead(inputFilename))
        //    using (TextReader tr = new StreamReader(fs))
        //    {
        //        List<IntermediateTags> intermediateStates = new List<IntermediateTags>();
        //        while (tr.Peek() > -1)
        //        {
        //            string line = tr.ReadLine();
        //            NGramTags ngt = ParseTags(line, ref intermediateStates);
        //            if (ngt != null)
        //            {
        //                string nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
        //                if (dictionaryEntryTable.Count() < 50000)
        //                {
        //                    dictionaryEntryTable.Add(new Models.DictionaryEntry(nGramEntryTable[nGramString], nGramTagTable[ngt.GetHash()], 0));
        //                }
        //            }
        //        }
        //    }
        //}

            

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string inputFilename;
    }

}
