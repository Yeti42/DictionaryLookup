namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NGramEntries",
                c => new
                    {
                        NGramEntryID = c.Long(nullable: false, identity: true),
                        WordID = c.Long(nullable: false),
                        Previous1WordID = c.Long(nullable: false),
                        Previous2WordID = c.Long(nullable: false),
                        NGram = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NGramEntryID);
            
            CreateTable(
                "dbo.WordStrings",
                c => new
                    {
                        WordStringID = c.Long(nullable: false, identity: true),
                        Word = c.String(),
                    })
                .PrimaryKey(t => t.WordStringID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WordStrings");
            DropTable("dbo.NGramEntries");
        }
    }
}
