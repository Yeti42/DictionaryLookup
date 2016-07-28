using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class Languages
    {
        public Int16 LanguagesID { get; set; }
        public string BCP47 { get; set;  }
        public string FriendlyName { get; set; }
    }
}