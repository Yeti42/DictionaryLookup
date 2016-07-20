using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace DictionaryLookup.Models
{
    public class DictionaryWord
    {
        public DictionaryWord() { }
        public DictionaryWord(string word, bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCallig, Int16 hwrCost)
        {
            Word = word;
            Restricted = restricted;
            SpellerFrequency = frequency;
            TextPredictionCost = stopCost;
            TextPredictionBackOffCost = backoffCost;
            TextPredictionBadWord = badWord;
            HWRCalligScore = hwrCallig;
            HWRWordCost = hwrCost;
        }

        public int DictionaryWordID { get; set; }
        public string Word { get; set; }

        // Tag 0
        public bool Restricted { get; set; }
        //public string Dialect { get; set; }
        public Int16 SpellerFrequency { get; set; }

        // Tag 1
        public bool TextPredictionBadWord { get; set; }
        public Int16 TextPredictionCost { get; set; }
        public Int16 TextPredictionBackOffCost { get; set; }

        // Tag 5
        public Int16 HWRCalligScore { get; set; }
        public Int16 HWRWordCost { get; set; }

        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
                "api/words/{0}", this.DictionaryWordID);
            }
            set { }
        }
    }
}