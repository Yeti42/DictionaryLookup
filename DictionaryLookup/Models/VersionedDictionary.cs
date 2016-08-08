using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class VersionedDictionary
    {
        public VersionedDictionary()
        {
            Set(0, "");
        }
        public VersionedDictionary(Int16 langID, string verName)
        {
            Set(langID, verName);
        }
        private void Set(Int16 langID, string verName)
        {
            LanguageID = langID;
            VersionName = verName;
        }
        public Int32 VersionedDictionaryID { get; set; }
        public Int16 LanguageID { get; set; }
        public string VersionName { get; set; }
    }
}