namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NGramEntries", "NGramTagID", c => c.Long(nullable: false));
            AddColumn("dbo.NGramEntries", "VersionedDictionaryID", c => c.Int(nullable: false));
            DropTable("dbo.DictionaryEntries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DictionaryEntries",
                c => new
                    {
                        DictionaryEntryID = c.Long(nullable: false, identity: true),
                        NGramEntryID = c.Long(nullable: false),
                        NGramTagID = c.Long(nullable: false),
                        VersionedDictionaryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DictionaryEntryID);
            
            DropColumn("dbo.NGramEntries", "VersionedDictionaryID");
            DropColumn("dbo.NGramEntries", "NGramTagID");
        }
    }
}
