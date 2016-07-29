using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class WordString
    {
        public WordString()
        {
            WordStringID = 0;
            Word = "";
        }
        public WordString(string word)
        {
            WordStringID = 0;
            Word = word;
        }
        public Int64 WordStringID { get; set; }
        public string Word { get; set; }
    }
}