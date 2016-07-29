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

        public NGramEntry(Int64 wid, Int64 p1id, Int64 p2Id)
        {
            Set(wid, p1id, p2Id);
        }

        public void Set(Int64 wid, Int64 p1id, Int64 p2Id)
        {
            NGramEntryID = 0;
            WordID = wid;
            Previous1WordID = p1id;
            Previous2WordID = p2Id;
            if (p2Id > 0) NGram = 3;
            else if (p1id > 0) NGram = 2;
            else NGram = 1;
        }

        public Int64 NGramEntryID { get; set; }
        public Int64 WordID { get; set; }
        public Int64 Previous1WordID { get; set; }
        public Int64 Previous2WordID { get; set; }

        //Meta-data
        public int  NGram { get; set; }
    }
}