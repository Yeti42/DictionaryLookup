using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class DictionaryErrorReport
    {
        public DictionaryErrorReport()
        {
            Set(0, 0, 0, "");
        }
        public DictionaryErrorReport(Int64 id, Int16 err, Int32 user, string note)
        {
            Set(id, err, user, note);
        }

        private void Set(Int64 id, Int16 err, Int32 user, string note)
        {
            DictionaryErrorReportId = 0;
            NGramStringID = id;
            NGramTagID = 1; // Only en-US for now.
            VersionedDictionaryID = 1; // Versioning placeholder.
            ReportDateTime = DateTime.Now;
            ErrorTypeID = err;
            UserID = user;
            Notes = note;
        }

        public Int32 DictionaryErrorReportId { get; set; }
        public Int32 VersionedDictionaryID { get; set; }
        public Int64 NGramStringID { get; set; }
        public Int64 NGramTagID { get; set; } 
        public DateTime ReportDateTime { get; set; }
        public int ErrorTypeID { get; set; } // key to the errortype table
        public Int32 UserID { get; set; } // key to the user table
        public string Notes { get; set; }
    }
}