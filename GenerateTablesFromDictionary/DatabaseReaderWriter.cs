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
        private static bool overwriteDBFiles = false;
        private static string bcpProgram = "C:\\Program Files (x86)\\Microsoft SQL Server\\Client SDK\\ODBC\\130\\Tools\\Binn\\bcp.exe";
        private static string opdir = "C:\\temp\\";
        private string serverName = @"osgdictionaries.database.windows.net";
        private string databaseName = @"Dictionaries";
        private bool useRemoteDB = true;

        public DatabaseReaderWriter(bool remote = true)
        {
            useRemoteDB = remote;
            if (useRemoteDB)
            {
                serverName = @"osgdictionaries.database.windows.net";
                databaseName = @"Dictionaries";
            }
            else
            {
                serverName = @"(localdb)\MSSQLLocalDB";
                databaseName = @"DictionaryLookupContext-20160713172838";
            }
        }

        public void ParseTestTrieFile(string filename)
        {
            GetCurrentWordStrings();
            GetCurrentNGramTags();
            GetCurrentNGrams();

            HashSet<string> newWordStrings = new HashSet<string>();
            HashSet<Int64> newNGramTags = new HashSet<long>();
            HashSet<string> newNGrams = new HashSet<string>();
            using (TestTrieReader ttr = new TestTrieReader(filename))
            {
                while (ttr.Next())
                {
                    if (!existingWords.ContainsKey(ttr.w[0]) && !newWordStrings.Contains(ttr.w[0])) newWordStrings.Add(ttr.w[0]);
                    if (!existingWords.ContainsKey(ttr.w[1]) && !newWordStrings.Contains(ttr.w[1])) newWordStrings.Add(ttr.w[1]);
                    if (!existingWords.ContainsKey(ttr.w[2]) && !newWordStrings.Contains(ttr.w[2])) newWordStrings.Add(ttr.w[2]);
                    if (!existingTags.ContainsKey(ttr.nGramTags.GetHash()) && !newNGramTags.Contains(ttr.nGramTags.GetHash())) newNGramTags.Add(ttr.nGramTags.GetHash());
                    if (!existingNGrams.ContainsKey(ttr.nGramString) && !newNGrams.Contains(ttr.nGramString)) newNGrams.Add(ttr.nGramString);
                }
            }
            SetNewWordStrings(ref newWordStrings);
            SetNewNGramTags(ref newNGramTags);
            SetNewNGrams(ref newNGrams);
            SetNewNGramEntries(filename);
        }

        public void UploadNewTableEntries()
        {
            Upload("VersionedDictionaries");
            Upload("WordStrings");
            Upload("NGramTags");
            Upload("NGramStrings");
            Upload("NGramEntries");
        }

        private void Upload(string tableName)
        {
            string filename = String.Concat(opdir, tableName, ".New.txt");
            if (File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                if (fi.Length > 0)
                {
                    RunBCP(tableName, "", false);
                }
            }
        }

        private string RunBCP(string tableName, string query = "", bool download=true)
        {
            string filename = String.Concat(opdir, tableName, download?".txt":".New.txt");
            if (overwriteDBFiles || (download && !File.Exists(filename)) || (!download && File.Exists(filename)))
            {
                StringBuilder args = new StringBuilder();
                if (useRemoteDB && (pwd.Length == 0))
                {
                    Console.Write("Database Password: ");
                    pwd = Console.ReadLine();
                }
                if (download)
                {
                    if (query.Length > 0)
                    {
                        args.AppendFormat("\"{0}\"", query);
                    }
                    else
                    {
                        args.AppendFormat("\"select * from {0}\"", tableName);
                    }
                }
                else
                {
                    args.Append(tableName);
                }
                args.AppendFormat(" {0} {1}", download ? " queryout " : " in ", filename); // Local file
                args.AppendFormat(" -S{0} -w -d{1}", serverName, databaseName);  // Database connection
                if (useRemoteDB)
                {
                    args.AppendFormat(" -Utpg -P \"{0}\"", pwd);  // Credentials
                }
                else
                {
                    args.Append(" -T");
                }
                Process p = Process.Start(bcpProgram, args.ToString());
                p.WaitForExit();
            }
            return filename;
        }

        private void GetCurrentWordStrings()
        {
            string tableName = "WordStrings";
            using (FileStream fs = File.OpenRead(RunBCP(tableName)))
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
        private void GetCurrentNGramTags()
        {
            string tableName = "NGramTags";
            NGramTags ngt = new NGramTags();
            using (FileStream fs = File.OpenRead(RunBCP(tableName)))
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

        private void GetCurrentNGrams()
        {
            string tableName = "NGramStrings";

            Dictionary<Int64, string> wordIndexLookup = new Dictionary<long, string>();
            foreach (string w in existingWords.Keys) wordIndexLookup[existingWords[w]] = w;
            using (FileStream fs = File.OpenRead(RunBCP(tableName)))
            using (TextReader tr = new StreamReader(fs))
            {
                StringBuilder nGramString = new StringBuilder();
                while (tr.Peek() > -1)
                {
                    string line = tr.ReadLine();
                    Int64 id = Int64.Parse(line.Remove(line.IndexOf('\t')));
                    string[] idxs = line.Remove(line.LastIndexOf('\t')).Substring(line.IndexOf('\t') + 1).Split('\t');
                    nGramString.Clear();
                    nGramString.AppendFormat("{0} {1} {2}", wordIndexLookup[Int32.Parse(idxs[2])], wordIndexLookup[Int32.Parse(idxs[1])], wordIndexLookup[Int32.Parse(idxs[0])]);
                    existingNGrams[nGramString.ToString().TrimStart(' ')] = id;
                    maxNGramsID = Math.Max(maxNGramsID, id);
                }
            }
        }

        private void SetNewWordStrings(ref HashSet<string> newWordStrings)
        {
            string tableName = "WordStrings";
            using (FileStream fs = File.Create(String.Concat(opdir, tableName, ".New.txt")))
            using (TextWriter tw = new StreamWriter(fs, Encoding.Unicode))
            {
                // Ensure that the "" string is #1
                if (!existingWords.ContainsKey("") )
                {
                    if(maxWordStringsID != 0) throw(new Exception("Existing words does not contain empty string"));
                    tw.WriteLine("1\t");
                    newWordStrings.Remove("");
                    maxWordStringsID = 1;
                    existingWords[""] = 1;
                }
                foreach (string k in newWordStrings)
                {
                    maxWordStringsID++;
                    existingWords[k] = maxWordStringsID;
                    tw.WriteLine("{0}\t{1}", maxWordStringsID.ToString(), k);
                }
            }
        }

        private void SetNewNGramTags(ref HashSet<Int64> newNGramTags)
        {
            string tableName = "NGramTags";
            using (FileStream fs = File.Create(String.Concat(opdir, tableName, ".New.txt")))
            using (TextWriter tw = new StreamWriter(fs, Encoding.Unicode))
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
        private void SetNewNGrams(ref HashSet<string> newNGrams)
        {
            string tableName = "NGramStrings";
            using (FileStream fs = File.Create(String.Concat(opdir, tableName, ".New.txt")))
            using (TextWriter tw = new StreamWriter(fs, Encoding.Unicode))
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

        private void SetNewNGramEntries(string filename)
        {
            string tableName = "NGramEntries";
            using (FileStream fs = File.OpenRead(RunBCP(tableName, "select top 1 NGramEntryID from NGramEntries order by NGramEntryID desc")))
            using (TextReader tr = new StreamReader(fs))
            {
                maxNGramsEntriesID = (tr.Peek() > -1) ? Int32.Parse(tr.ReadLine()) : 0;
            }

            using (TestTrieReader ttr = new TestTrieReader(filename))
            using (FileStream fs = File.Create(String.Concat(opdir, tableName, ".New.txt")))
            using (TextWriter tw = new StreamWriter(fs, Encoding.Unicode))
            {
                while (ttr.Next())
                {
                    maxNGramsEntriesID++;
                    Int64 ngid = existingNGrams[ttr.nGramString];
                    Int64 tgid = existingTags[ttr.nGramTags.GetHash()];
                    tw.WriteLine("{0}\t{1}\t{2}\t{3}", maxNGramsEntriesID.ToString(), ngid.ToString(), tgid.ToString(), versionID);
                }
            }
        }

        public void SetVersionDictionaryID(int verID, string verName, string languageName)
        {
            string tableName = "VersionedDictionaries";
            if (verID == 0)
            {
                using (FileStream fs = File.OpenRead(RunBCP(tableName, "select top 1 VersionedDictionaryID from VersionedDictionaries order by VersionedDictionaryID desc")))
                using (TextReader tr = new StreamReader(fs))
                {
                    versionID = Int32.Parse(tr.ReadLine()) + 1;
                }
                using (FileStream fs = File.Create(String.Concat(opdir, tableName, ".New.txt")))
                using (TextWriter tw = new StreamWriter(fs, Encoding.Unicode))
                {
                    if(verName.Length == 0)
                    {
                        verName = string.Concat("Unnamed Dictionary ", DateTime.Now.ToString());
                    }
                    tw.WriteLine("{0}\t{1}\t{2}", versionID.ToString(), GetLanguageID(languageName).ToString(), verName);
                }
            }
            else
            {
                versionID = verID;
            }
        }

        private Int32 GetLanguageID(string languageName)
        {
            string tableName = "Languages";
            using (FileStream fs = File.OpenRead(RunBCP(tableName, String.Concat("select top 1 LanguagesID from Languages where BCP47 = '", languageName, "'"))))
            using (TextReader tr = new StreamReader(fs))
            {
                return (tr.Peek() > -1) ? Int32.Parse(tr.ReadLine()) : 0;
            }
        }

        private Dictionary<string, Int64> existingWords = new Dictionary<string, Int64>();
        //private Dictionary<string, Int64> newWords = new Dictionary<string, Int64>();
        private Dictionary<Int64, Int64> existingTags = new Dictionary<Int64, Int64>();
        //private Dictionary<Int64, Int64> newTags = new Dictionary<Int64, Int64>();
        private Dictionary<string, Int64> existingNGrams = new Dictionary<string, Int64>();
        //private Dictionary<string, Int64> newNGrams = new Dictionary<string, Int64>();

        private Int64 maxWordStringsID = 0;
        private Int64 maxNGramTagsID = 0;
        private Int64 maxNGramsID = 0;
        private Int64 maxNGramsEntriesID = 0;

        private string pwd = "";

        private int versionID = 0;
    }
}
