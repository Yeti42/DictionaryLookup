using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class DictionaryReport
    {
        public DictionaryReport()
        {
            DictionaryReportId = 0;
            WordID = 0;
            LanguageID = 1;
            DictionaryVersion = 1;
            ReportDateTime = DateTime.Now;
            ErrorTypeID = 1;
            UserID = 1;
            Notes = "";
        }
        public DictionaryReport(Int64 id, Int16 err, Int32 user, string note)
        {
            DictionaryReportId = 0;
            WordID = id;
            LanguageID = 1; // Only en-US for now.
            DictionaryVersion = 1; // Versioning placeholder.
            ReportDateTime = DateTime.Now;
            ErrorTypeID = err;
            UserID = user;
            Notes = note;
        }

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