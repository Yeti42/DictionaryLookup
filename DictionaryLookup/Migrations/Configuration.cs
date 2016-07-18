namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DictionaryLookup.Models;
    using System.IO;
    using System.Collections;

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
               new DictionaryWord("Hello", 4),
               new DictionaryWord("hello", 6),
               new DictionaryWord("Goodbye", 77),
               new DictionaryWord("testing", 43)
               //new DictionaryWord("'twas a")
               //new DictionaryWord("'twas a	|0 = 0x0000457c")
            );

            string [] alllines = File.ReadAllLines(@"S:\RS\dev\base\win32\winnls\ELS\AdvancedServices\Spelling\nlg7\spellers\dictionaries\english_united_states\dump2.txt");
            
            int count = 0;
            foreach (string line in alllines)
            {
                if (count++ > 40)
                {
                    return;
                }
                context.DictionaryWords.Add(new DictionaryWord(line));
            }
        }
    }
}
