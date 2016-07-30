using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class DictionaryEntry
    {
        public DictionaryEntry()
        {
            DictionaryEntryID = 0;
            NGramEntryID = 0;
            NGramTagID = 0;
        }
        public DictionaryEntry(Int64 ngeID, Int64 ngtID)
        {
            DictionaryEntryID = 0;
            NGramEntryID = ngeID;
            NGramTagID = ngtID;
        }
        public Int64 DictionaryEntryID { get; set; }
        public Int64 NGramEntryID { get; set; }
        public Int64 NGramTagID { get; set; }
    }
}