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
        public DictionaryWord(string word, int Tag0)
        {
            Word = word;
            Restricted = Convert.ToString((Tag0 >> 4) & 0xF, 2).PadLeft(4, '0');
            Dialect = Convert.ToString(Tag0 & 0xF, 2).PadLeft(4, '0');
            SpellerFrequency = Convert.ToString((Tag0 >> 6) & 0xF, 2).PadLeft(2, '0');
        }
        public DictionaryWord(string line)
        {
            string[] words = line.Split('\t');

            Restricted = "0000";
            Dialect = "1111";
            SpellerFrequency = "00";

            foreach (string word in words)
            {
                if(word == words.First())
                {
                    Word = word;
                }
                else
                {
                    string[] tag = word.Split('=');
                    tag[0] = tag[0].Replace("|", " ");
                    //Word = Word + tag[0] + tag[1];
                    Int32 tagID = Convert.ToInt32(tag[0]);
                    Int32 tagValue = Int32.Parse(tag[1].Remove(0,tag[1].IndexOf('x')+1), System.Globalization.NumberStyles.HexNumber);
                    if (tagID == 0)
                    {
                        // Dictionary Tag
                        Restricted = Convert.ToString((tagValue >> 4) & 0xF, 2).PadLeft(4, '0');
                        Dialect = Convert.ToString(tagValue & 0xF, 2).PadLeft(4, '0');
                        SpellerFrequency = Convert.ToString((tagValue >> 6) & 0xF, 2).PadLeft(2, '0');
                    }
                    else if (tagID == 1)
                    {
                        // Text Prediction Tag
                        TextPredictionBadWord = Convert.ToString((tagValue >> 24) & 0x1);
                        TextPredictionCost = Convert.ToString((tagValue >> 16) & 0xFF);
                        TextPredictionBackOffCost = Convert.ToString((tagValue >> 8) & 0xFF);
                    }
                }
            }
        }
        public int DictionaryWordID { get; set; }
        public string Word { get; set; }

        // Tag 0
        public string Restricted { get; set; }
        public string Dialect { get; set; }
        public string SpellerFrequency { get; set; }

        // Tag 1
        public string TextPredictionBadWord { get; set; }
        public string TextPredictionCost { get; set; }
        public string TextPredictionBackOffCost { get; set; }

        // Tag 5
        public string HandwritingCalligScore { get; set; }
        public string HandwritingWordCost { get; set; }

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