namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DictionaryLookup.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<DictionaryLookup.Models.DictionaryLookupContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        
        protected override void Seed(DictionaryLookup.Models.DictionaryLookupContext context)
        {
            // Deletes all data, from all tables, except for __MigrationHistory
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            context.DictionaryWords.AddOrUpdate(p => p.Word,
               new DictionaryWord
               {
                   Word = "Hello",
                   Tag0 = 32767
               },
               new DictionaryWord
               {
                   Word = "hello",
                   Tag0 = 32767
               },
               new DictionaryWord
               {
                   Word = "Goodbye",
                   Tag0 = 32767
               },
               new DictionaryWord
               {
                   Word = "testing",
                   Tag0 = 32767
               }
            );
        }
    }
}
