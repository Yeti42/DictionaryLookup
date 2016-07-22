namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DictionaryWords",
                c => new
                    {
                        DictionaryWordID = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                        Restricted = c.Boolean(nullable: false),
                        SpellerFrequency = c.Short(nullable: false),
                        TextPredictionBadWord = c.Boolean(nullable: false),
                        TextPredictionCost = c.Short(nullable: false),
                        TextPredictionBackOffCost = c.Short(nullable: false),
                        HWRCalligScore = c.Short(nullable: false),
                        HWRWordCost = c.Short(nullable: false),
                        Self = c.String(),
                    })
                .PrimaryKey(t => t.DictionaryWordID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DictionaryWords");
        }
    }
}
