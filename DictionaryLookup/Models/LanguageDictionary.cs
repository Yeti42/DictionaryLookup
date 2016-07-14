using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace DictionaryLookup.Models
{
    public class LanguageDictionary
    {
        public int LanguageDictionaryID { get; set; }
        public string BCP47 { get; set; }
        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/dictionary/{0}", this.BCP47);
            }
            set { }
        }
    }
}