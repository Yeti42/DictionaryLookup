using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class NGramTags
    {
        public NGramTags()
        {
            Set(false, 0, 0, 0, false, 4091, 0);
        }

        private void Set(bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCost, Int16 hwrCallig)
        {
            Restricted = restricted;
            SpellerFrequency = frequency;
            TextPredictionCost = (Byte)stopCost;
            TextPredictionBackOffCost = (Byte)backoffCost;
            TextPredictionBadWord = badWord;
            HWRCost = hwrCost;
            HWRCalligraphyCost = hwrCallig;
        }
        
        public Int64 NGramTagsID { get; set; }

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
    }
}