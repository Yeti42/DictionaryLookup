using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class NGramEntry
    {
        public NGramEntry()
        {
            Set(0, 0, 0);
        }

        public NGramEntry(Int64 ngsID, Int64 ngtID, Int32 vdID)
        {
            Set(ngsID, ngtID, vdID);
        }

        public void Set(Int64 ngsID, Int64 ngtID, Int32 vdID)
        {
            NGramEntryID = 0;
            NGramStringID = ngsID;
            NGramTagID = ngtID;
            VersionedDictionaryID = vdID;
        }

        public Int64 NGramEntryID { get; set; }
        public Int64 NGramStringID { get; set; }
        public Int64 NGramTagID { get; set; }
        public Int32 VersionedDictionaryID { get; set; }
    }
}