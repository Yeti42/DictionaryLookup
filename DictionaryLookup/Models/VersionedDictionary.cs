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
        public VersionedDictionary(int langID, string verName)
        {
            Set(langID, verName);
        }
        private void Set(int langID, string verName)
        {
            LanguageID = langID;
            VersionName = verName;
        }
        public int VersionedDictionaryID { get; set; }
        public int LanguageID { get; set; }
        public string VersionName { get; set; }
    }
}