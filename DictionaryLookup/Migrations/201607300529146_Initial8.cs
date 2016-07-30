namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DictionaryEntries",
                c => new
                    {
                        DictionaryEntryID = c.Long(nullable: false, identity: true),
                        NGramEntryID = c.Long(nullable: false),
                        NGramTagID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.DictionaryEntryID);
            
            CreateTable(
                "dbo.NGramTags",
                c => new
                    {
                        NGramTagsID = c.Long(nullable: false, identity: true),
                        Restricted = c.Boolean(nullable: false),
                        SpellerFrequency = c.Short(nullable: false),
                        TextPredictionBadWord = c.Boolean(nullable: false),
                        TextPredictionCost = c.Byte(nullable: false),
                        TextPredictionBackOffCost = c.Byte(nullable: false),
                        HWRCost = c.Short(nullable: false),
                        HWRCalligraphyCost = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.NGramTagsID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NGramTags");
            DropTable("dbo.DictionaryEntries");
        }
    }
}
