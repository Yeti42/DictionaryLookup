using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class DictionaryEntry
    {
        public Int64 DictionaryEntryID { get; set; }
        public Int64 NGramEntryID { get; set; }
        public Int64 NGramTagID { get; set; }
    }
}