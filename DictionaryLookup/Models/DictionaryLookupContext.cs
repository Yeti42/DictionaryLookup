using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DictionaryLookup.Models
{
    public class DictionaryLookupContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DictionaryLookupContext() : base("name=DictionaryLookupContext")
        {
        }

        public System.Data.Entity.DbSet<DictionaryLookup.Models.DictionaryWord> DictionaryWords { get; set; }

        public System.Data.Entity.DbSet<DictionaryLookup.Models.Languages> Languages { get; set; }

        public System.Data.Entity.DbSet<DictionaryLookup.Models.DictionaryReport> DictionaryReports { get; set; }

        public System.Data.Entity.DbSet<DictionaryLookup.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<DictionaryLookup.Models.ErrorType> ErrorTypes { get; set; }
    }
}
