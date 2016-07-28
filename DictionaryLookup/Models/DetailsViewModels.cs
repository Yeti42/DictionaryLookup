using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class ErrorReportViewModel
    {
        public ErrorReportViewModel(DictionaryReport dr1, string e1, string u1, string l1, string w1)
        {
            dr = dr1;
            errorName = e1;
            userName = u1;
            languageName = l1;
            wordString = w1;
        }
        public DictionaryReport dr { get; set; }
        public string errorName { get; set; }
        public string userName { get; set; }
        public string languageName { get; set; }
        public string wordString { get; set; }
    }
}