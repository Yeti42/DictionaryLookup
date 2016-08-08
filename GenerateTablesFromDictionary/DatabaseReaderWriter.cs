﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Collections;

namespace GenerateTablesFromDictionary
{
    class DatabaseReaderWriter
    {
        public void GetCurrentWordStrings(bool downloadTable = true)
        {
            if (downloadTable)
            {
                Process p = Process.Start(bcpProgram, "\"select * from WordStrings\" queryout C:\\temp\\WordStrings.txt -Sosgdictionaries.database.windows.net -Utpg -c -dDictionaries");
                p.WaitForExit();
            }
            
            using (FileStream fs = File.OpenRead("C:\\temp\\WordStrings.txt"))
            using (TextReader tr = new StreamReader(fs))
            {
                while (tr.Peek() > -1)
                {
                    string line = tr.ReadLine();
                    Int64 id = Int64.Parse(line.Remove(line.IndexOf('\t')));
                    string wd = line.Substring(line.IndexOf('\t') + 1);
                    if (wd == "\0") wd = ""; // Empty string shows up as \0
                    existingWords[wd] = id;
                    maxWordStringsID = Math.Max(maxWordStringsID, id);
                }
            }
        }
        public void GetCurrentNGramTags(bool downloadTable = true)
        {
            if (downloadTable)
            {
                Process p = Process.Start(bcpProgram, "\"select * from NGramTags\" queryout C:\\temp\\NGramTags.txt -Sosgdictionaries.database.windows.net -Utpg -c -dDictionaries");
                p.WaitForExit();
            }

            NGramTags ngt = new NGramTags();
            using (FileStream fs = File.OpenRead("C:\\temp\\NGramTags.txt"))
            using (TextReader tr = new StreamReader(fs))
            {
                while (tr.Peek() > -1)
                {
                    string line = tr.ReadLine();
                    string[] tags = line.Split('\t');
                    Int64 id = Int64.Parse(tags[0]);
                    ngt.Set(tags[1] == "1", Int16.Parse(tags[2]), Int16.Parse(tags[4]), Int16.Parse(tags[5]), tags[3] == "1", Int16.Parse(tags[6]), Int16.Parse(tags[7]));
                    existingTags.Add(ngt.GetHash(), id);
                    maxNGramTagsID = Math.Max(maxNGramTagsID, id);
                }
            }
        }

        public void GetCurrentNGrams(bool downloadTable = true)
        {
            if (downloadTable)
            {
                Process p = Process.Start(bcpProgram, "\"select * from NGramStrings\" queryout C:\\temp\\NGramStrings.txt -Sosgdictionaries.database.windows.net -Utpg -c -dDictionaries");
                p.WaitForExit();
            }
            using (FileStream fs = File.OpenRead("C:\\temp\\NGramStrings.txt"))
            using (TextReader tr = new StreamReader(fs))
            {
                while (tr.Peek() > -1)
                {
                    string line = tr.ReadLine();
                    Int64 id = Int64.Parse(line.Remove(line.IndexOf('\t')));
                    string nGramWordIndexes = line.Substring(line.IndexOf('\t') + 1);
                    existingNGrams[nGramWordIndexes] = id;
                    maxNGramsID = Math.Max(maxNGramsID, id);
                }
            }
        }

        public void SetNewWordStrings(ref HashSet<string> newWordStrings)
        {
            using (FileStream fs = File.Create("C:\\temp\\WordStrings.New.txt"))
            using (TextWriter tw = new StreamWriter(fs))
            {
                foreach (string k in newWordStrings)
                {
                    maxWordStringsID++;
                    existingWords[k] = maxWordStringsID;
                    tw.WriteLine("{0}\t{1}", maxWordStringsID.ToString(), k);
                }
            }
        }

        public void SetNewNGramTags(ref HashSet<Int64> newNGramTags)
        {
            using (FileStream fs = File.Create("C:\\temp\\NGramTags.New.txt"))
            using (TextWriter tw = new StreamWriter(fs))
            {
                NGramTags ngt = new NGramTags();
                foreach (Int64 k in newNGramTags)
                {
                    maxNGramTagsID++;
                    existingTags[k] = maxNGramTagsID;
                    ngt.Set(k);
                    tw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
                        maxNGramTagsID.ToString(),
                        ngt.Restricted ? "1" : "0",
                        ngt.SpellerFrequency.ToString(),
                        ngt.TextPredictionBadWord ? "1" : "0",
                        ngt.TextPredictionCost.ToString(),
                        ngt.TextPredictionBackOffCost.ToString(),
                        ngt.HWRCost.ToString(),
                        ngt.HWRCalligraphyCost.ToString());
                }
            }
        }
        public void SetNewNGrams(ref HashSet<string> newNGrams)
        {
            using (FileStream fs = File.Create("C:\\temp\\NGramStrings.New.txt"))
            using (TextWriter tw = new StreamWriter(fs))
            {
                foreach (string k in newNGrams)
                {
                    maxNGramsID++;
                    existingNGrams[k] = maxNGramsID;
                    string[] w = new string[3];

                    w[1] = ""; w[2] = "";
                    string[] words = k.Split(' ');
                    int wordCount = words.Count();
                    w[0] = words[wordCount - 1];
                    if (wordCount > 1)
                    {
                        w[1] = words[wordCount - 2];
                    }
                    if (wordCount > 2)
                    {
                        w[2] = words[wordCount - 3];
                        // Prepend all the other words to the p2 in the case of quad-grams and above
                        for (int i = wordCount - 4; i >= 0; i--)
                        {
                            w[2] = words[i] + " " + w[2];
                        }
                        wordCount = 3;
                    }

                    tw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", maxNGramsID.ToString(), existingWords[w[0]], existingWords[w[1]], existingWords[w[2]], words.Count());
                }
            }
        }


        private string bcpProgram = "C:\\Program Files (x86)\\Microsoft SQL Server\\Client SDK\\ODBC\\130\\Tools\\Binn\\bcp.exe";

        public Dictionary<string, Int64> existingWords = new Dictionary<string, Int64>();
        //private Dictionary<string, Int64> newWords = new Dictionary<string, Int64>();
        public Dictionary<Int64, Int64> existingTags = new Dictionary<Int64, Int64>();
        //private Dictionary<Int64, Int64> newTags = new Dictionary<Int64, Int64>();
        public Dictionary<string, Int64> existingNGrams = new Dictionary<string, Int64>();
        //private Dictionary<string, Int64> newNGrams = new Dictionary<string, Int64>();

        public Int64 maxWordStringsID = 0;
        public Int64 maxNGramTagsID = 0;
        public Int64 maxNGramsID = 0;
    }
}
