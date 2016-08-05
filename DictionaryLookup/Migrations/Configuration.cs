namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DictionaryLookup.Models;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    internal sealed class Configuration : DbMigrationsConfiguration<DictionaryLookup.Models.DictionaryLookupContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DictionaryLookup.Models.DictionaryLookupContext context)
        {
            // Languages
            // Error Types
            // VersionedDictionaries
            // WordStrings. First entry ""

            return;
        }
    }
}
