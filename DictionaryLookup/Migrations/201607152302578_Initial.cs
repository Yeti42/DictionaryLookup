namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DictionaryWords", "Dialect", c => c.String());
            AddColumn("dbo.DictionaryWords", "SpellerFrequency", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DictionaryWords", "SpellerFrequency");
            DropColumn("dbo.DictionaryWords", "Dialect");
        }
    }
}
