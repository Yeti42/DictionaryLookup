namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NGramEntries",
                c => new
                    {
                        NGramEntryID = c.Long(nullable: false, identity: true),
                        NGramStringID = c.Long(nullable: false),
                        NGramTagsID = c.Long(nullable: false),
                        VersionedDictionaryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NGramEntryID);
            
            AlterColumn("dbo.DictionaryErrorReports", "ErrorTypeID", c => c.Int(nullable: false));
            AlterColumn("dbo.VersionedDictionaries", "LanguageID", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VersionedDictionaries", "LanguageID", c => c.Int(nullable: false));
            AlterColumn("dbo.DictionaryErrorReports", "ErrorTypeID", c => c.Short(nullable: false));
            DropTable("dbo.NGramEntries");
        }
    }
}
