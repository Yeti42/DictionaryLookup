namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DictionaryWords", "HWRCalligScore", c => c.Short(nullable: false));
            AddColumn("dbo.DictionaryWords", "HWRWordCost", c => c.Short(nullable: false));
            AlterColumn("dbo.DictionaryWords", "Restricted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.DictionaryWords", "SpellerFrequency", c => c.Short(nullable: false));
            AlterColumn("dbo.DictionaryWords", "TextPredictionBadWord", c => c.Boolean(nullable: false));
            AlterColumn("dbo.DictionaryWords", "TextPredictionCost", c => c.Short(nullable: false));
            AlterColumn("dbo.DictionaryWords", "TextPredictionBackOffCost", c => c.Short(nullable: false));
            DropColumn("dbo.DictionaryWords", "Dialect");
            DropColumn("dbo.DictionaryWords", "HandwritingCalligScore");
            DropColumn("dbo.DictionaryWords", "HandwritingWordCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DictionaryWords", "HandwritingWordCost", c => c.String());
            AddColumn("dbo.DictionaryWords", "HandwritingCalligScore", c => c.String());
            AddColumn("dbo.DictionaryWords", "Dialect", c => c.String());
            AlterColumn("dbo.DictionaryWords", "TextPredictionBackOffCost", c => c.String());
            AlterColumn("dbo.DictionaryWords", "TextPredictionCost", c => c.String());
            AlterColumn("dbo.DictionaryWords", "TextPredictionBadWord", c => c.String());
            AlterColumn("dbo.DictionaryWords", "SpellerFrequency", c => c.String());
            AlterColumn("dbo.DictionaryWords", "Restricted", c => c.String());
            DropColumn("dbo.DictionaryWords", "HWRWordCost");
            DropColumn("dbo.DictionaryWords", "HWRCalligScore");
        }
    }
}
