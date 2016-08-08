using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace GenerateTablesFromDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseReaderWriter dbrw = new DatabaseReaderWriter();
            dbrw.GetCurrentWordStrings(false);
            dbrw.GetCurrentNGramTags(false);
            dbrw.GetCurrentNGrams(false);

            HashSet<string> newWordStrings = new HashSet<string>();
            HashSet<Int64> newNGramTags = new HashSet<long>();
            HashSet<string> newNGrams = new HashSet<string>();
            using (TestTrieReader ttr = new TestTrieReader(args[0]))
            {
                while (ttr.Next())
                {
                    if (!dbrw.existingWords.ContainsKey(ttr.w[0]) && !newWordStrings.Contains(ttr.w[0])) newWordStrings.Add(ttr.w[0]);
                    if (!dbrw.existingWords.ContainsKey(ttr.w[1]) && !newWordStrings.Contains(ttr.w[1])) newWordStrings.Add(ttr.w[1]);
                    if (!dbrw.existingWords.ContainsKey(ttr.w[2]) && !newWordStrings.Contains(ttr.w[2])) newWordStrings.Add(ttr.w[2]);
                    if (!dbrw.existingTags.ContainsKey(ttr.nGramTags.GetHash()) && !newNGramTags.Contains(ttr.nGramTags.GetHash())) newNGramTags.Add(ttr.nGramTags.GetHash());
                    if (!dbrw.existingNGrams.ContainsKey(ttr.nGramString) && !newNGrams.Contains(ttr.nGramString)) newNGrams.Add(ttr.nGramString);
                }
            }
            dbrw.SetNewWordStrings(ref newWordStrings);
            dbrw.SetNewNGramTags(ref newNGramTags);
            dbrw.SetNewNGrams(ref newNGrams);
        }
    }
}
