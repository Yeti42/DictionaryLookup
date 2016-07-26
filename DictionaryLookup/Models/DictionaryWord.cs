using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace DictionaryLookup.Models
{
    public class DictionaryWord
    {
        public DictionaryWord()
        {
            Word = "";
            Restricted = false;
            SpellerFrequency = 0;
            TextPredictionCost = 0;
            TextPredictionBackOffCost = 0;
            TextPredictionBadWord = false;
            HWRCost = 4091;
            HWRCalligraphyCost = 0;
        }
        public DictionaryWord(string word, bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCost, Int16 hwrCallig)
        {
            Set(word, restricted, frequency, stopCost, backoffCost, badWord, hwrCost, hwrCallig);
        }

        public void Set(string word, bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCost, Int16 hwrCallig)
        {
            Word = word;
            Restricted = restricted;
            SpellerFrequency = frequency;
            TextPredictionCost = (Byte)stopCost;
            TextPredictionBackOffCost = (Byte)backoffCost;
            TextPredictionBadWord = badWord;
            HWRCost = hwrCost;
            HWRCalligraphyCost = hwrCallig;
            NGram = 1;
            foreach (char c in word) if (c == ' ') NGram++;
        }

        public Int64 DictionaryWordID { get; set; }
        public string Word { get; set; }

        // Tag 0
        public bool Restricted { get; set; }
        public Int16 SpellerFrequency { get; set; }

        // Tag 1
        public bool TextPredictionBadWord { get; set; }
        public Byte TextPredictionCost { get; set; }
        public Byte TextPredictionBackOffCost { get; set; }

        // Tag 5
        public Int16 HWRCost { get; set; }
        public Int16 HWRCalligraphyCost { get; set; }

        //Meta-data
        public Int16 NGram { get; set; }
        
    }
}