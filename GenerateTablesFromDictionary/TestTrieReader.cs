using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GenerateTablesFromDictionary
{
    public class TestTrieReader : IDisposable
    {
        public TestTrieReader(string inputFilename)
        {
            fs = System.IO.File.OpenRead(inputFilename);
            tr = new StreamReader(fs);
        }

        public bool Next()
        {
            while (tr.Peek() > -1)
            {
                string line = tr.ReadLine();
                NGramTags ngt = ParseTags(line, ref intermediateStates);
                if (ngt != null)
                {
                    nGramString = line.Contains("\t") ? line.Remove(line.IndexOf('\t')) : line;
                    nGramTags.Set(ngt.GetHash());
                    NGram = WordStringsFromNGramString(nGramString);
                    return true;
                }
            }
            return false;
        }


        private NGramTags ParseTags(string line, ref List<IntermediateTags> intermediateStates)
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

        private int WordStringsFromNGramString(string nGramString)
        {
            w[1] = ""; w[2] = "";
            string[] words = nGramString.Split(' ');
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
                return 3;
            }
            return wordCount;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
                fs.Dispose();
                tr.Dispose();
                intermediateStates = null;
                w = null;
                nGramTags = null;
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        struct IntermediateTags
        {
            public IntermediateTags(string s, Int16 c, Int16 n) { NGramLength = n; Cost = c; NGramString = s; }
            public string NGramString;
            public Int16 Cost;
            public Int16 NGramLength;
        }

        private FileStream fs = null;
        private TextReader tr = null;
        private List<IntermediateTags> intermediateStates = new List<IntermediateTags>();

        public NGramTags nGramTags = new NGramTags();
        public string nGramString;
        public string[] w = new string[3];
        public int NGram;
        bool disposed = false;
    }
}