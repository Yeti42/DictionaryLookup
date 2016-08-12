using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class ErrorReportViewModel
    {
        public ErrorReportViewModel()
        {
            dr = 0;
            errorName = "N/A";
            userName = "N/A";
            languageName = "N/A";
            reportDateTime = DateTime.Now;
            nGramString = "N/A";
            notes = "N/A";
        }
        public ErrorReportViewModel(Int32 dr1, string e1, string u1, string l1, DateTime dt1, string w1, string n1)
        {
            dr = dr1;
            errorName = e1;
            userName = u1;
            languageName = l1;
            reportDateTime = dt1;
            nGramString = w1;
            notes = n1;
        }
        public Int32 dr { get; set; }
        public string errorName { get; set; }
        public string userName { get; set; }
        public string languageName { get; set; }
        public DateTime reportDateTime { get; set; }
        public string nGramString { get; set; }
        public string notes { get; set; }
    }
}