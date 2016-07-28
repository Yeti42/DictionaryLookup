using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class DictionaryReport
    {
        public Int32 DictionaryReportId { get; set; }
        public Int64 WordID { get; set; } // key to the dictionary table
        public Int16 LanguageID { get; set; } // key to the languages table
        public Int16 DictionaryVersion { get; set; }
        public DateTime ReportDateTime { get; set; }
        public Int16 ErrorTypeID { get; set; } // key to the errortype table
        public Int32 UserID { get; set; } // key to the user table
        public string Notes { get; set; }
    }
}