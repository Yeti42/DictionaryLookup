﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class NGramTags
    {
        public NGramTags()
        {
            Restricted = false;
            SpellerFrequency = 0;
            TextPredictionCost = 0;
            TextPredictionBackOffCost = 0;
            TextPredictionBadWord = false;
            HWRCost = 4091;
            HWRCalligraphyCost = 0;
        }
        public NGramTags(bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCost, Int16 hwrCallig)
        {
            Set(restricted, frequency, stopCost, backoffCost, badWord, hwrCost, hwrCallig);
        }

        public NGramTags(string tagString, Int16 stopCost, Int16 backoffCost, bool badWord)
        {
            // Parse Tag0 for this NGram
            // Assume we've already filtered for the dialect bit
            bool spellerRestricted = false;
            Int16 spellerFrequency = 0;
            if (tagString.Contains("0=0x"))
            {
                spellerRestricted = ((Int32.Parse(tagString.Substring(tagString.IndexOf("0=0x") + 10, 1), System.Globalization.NumberStyles.HexNumber) & 1) > 0);
                spellerFrequency = (Int16)(Int32.Parse(tagString.Substring(tagString.IndexOf("0=0x") + 9, 1), System.Globalization.NumberStyles.HexNumber) & 3);
            }

            // Parse Tag5
            Int16 hwrCost = 4091; // Default HWR cost for a valid word
            Int16 hwrCallig = 0;
            if (tagString.Contains("5=0x"))
            {
                string tag5ValueString = tagString.Substring(tagString.IndexOf("5=0x") + 4, 8);
                hwrCost = Int16.Parse(tag5ValueString.Substring(4, 4), System.Globalization.NumberStyles.HexNumber);
                hwrCallig = (Int16)(Int32.Parse(tag5ValueString.Substring(5, 1), System.Globalization.NumberStyles.HexNumber) & 3);
            }

            Set(spellerRestricted, spellerFrequency, stopCost, backoffCost, badWord, hwrCost, hwrCallig);
        }

        public void Set(bool restricted, Int16 frequency, Int16 stopCost, Int16 backoffCost, bool badWord, Int16 hwrCost, Int16 hwrCallig)
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