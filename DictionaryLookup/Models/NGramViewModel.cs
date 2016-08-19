using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class NGramViewModel
    {
        public string NGramWordString { get; set; }
        public Int64 DictionaryNGramID { get; set; }
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