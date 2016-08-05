using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class NGramString
    {
        public Int64 NGramStringID { get; set; }
        public Int64 WordID { get; set; }
        public Int64 Previous1WordID { get; set; }
        public Int64 Previous2WordID { get; set; }
        public int NGram { get; set; }

    }
}