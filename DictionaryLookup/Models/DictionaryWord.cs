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
        public bool Restricted { get; set; }
        public double UnigramProbability { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/word/{0}", this.Word);
            }
            set { }
        }
    }
}