using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class NGramEntry
    {
        public Int64 NGramEntryID { get; set; }
        public Int64 NGramStringID { get; set; }
        public Int64 NGramTagsID { get; set; }
        public Int32 VersionedDictionaryID { get; set; }
    }
}