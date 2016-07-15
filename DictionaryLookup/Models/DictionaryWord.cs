using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace DictionaryLookup.Models
{
    public class DictionaryWord
    {
        public int DictionaryWordID { get; set; }
        public string Word { get; set; }
        public Int16 Tag0 { get; set; }
        public Int16 Tag1 { get; set; }
        public Int16 Tag2 { get; set; }
        public Int16 Tag3 { get; set; }
        public Int16 Tag4 { get; set; }
        public Int16 Tag5 { get; set; }
        public Int16 Tag6 { get; set; }
        public Int16 Tag7 { get; set; }
        public bool HasTag0 { get; set; }
        public bool HasTag1 { get; set; }
        public bool HasTag2 { get; set; }
        public bool HasTag3 { get; set; }
        public bool HasTag4 { get; set; }
        public bool HasTag5 { get; set; }
        public bool HasTag6 { get; set; }
        public bool HasTag7 { get; set; }

        public string Restricted
        {
            get
            {
                return Convert.ToString((Tag0 >> 4) & 0xF, 2).PadLeft(4, '0');
            }
            set
            {
                return;
            }
        }
        public string Dialect
        {
            get
            {
                return Convert.ToString(Tag0 & 0xF, 2).PadLeft(4, '0');
            }
            set
            {
                return;
            }
        }

        public string SpellerFrequency
        {
            get
            {
                return Convert.ToString((Tag0 >> 6) & 0x3, 2).PadLeft(2, '0');
            }
            set
            {
                return;
            }
        }

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