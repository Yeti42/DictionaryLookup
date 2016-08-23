namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DictionaryLookup.Models.DictionaryLookupContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DictionaryLookup.Models.DictionaryLookupContext context)
        {
            //  This method will be called after migrating to the latest version.

            // Languages
            /*
            context.Languages.AddOrUpdate(
                p => p.BCP47,
                new Models.Languages { BCP47 = "en-US", FriendlyName = "English(United States)" },
                new Models.Languages { BCP47 = "en-GB", FriendlyName = "English(Great Britain)" },
                new Models.Languages { BCP47 = "fr-FR", FriendlyName = "French(France)" },
                new Models.Languages { BCP47 = "it-IT", FriendlyName = "Italian" },
                new Models.Languages { BCP47 = "de-DE", FriendlyName = "German(Germany)" },
                new Models.Languages { BCP47 = "es-ES", FriendlyName = "Spanish(Spain)" },
                new Models.Languages { BCP47 = "es-MS", FriendlyName = "Spanish(Mexico)" }
            );

            // Error Types
            context.ErrorTypes.AddOrUpdate(
                p => p.ErrorTypeName,
                new Models.ErrorType { ErrorTypeID = 1, ErrorTypeName = "Misspelling" },
                new Models.ErrorType { ErrorTypeID = 2, ErrorTypeName = "Invalid" },
                new Models.ErrorType { ErrorTypeID = 3, ErrorTypeName = "Offensive" },
                new Models.ErrorType { ErrorTypeID = 4, ErrorTypeName = "Inoffensive" }
            );
            // VersionedDictionaries
            context.VersionedDictionaries.AddOrUpdate(
                p => p.LanguageID,
                new Models.VersionedDictionary { LanguageID = 1, VersionName = "Test en-US dictionary" }
            );

            // WordStrings. First entry ""
            context.WordStrings.AddOrUpdate(
                p => p.Word,
                new Models.WordString { Word = "" }
            );
            */
        }
    }
}
