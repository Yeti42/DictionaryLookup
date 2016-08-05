namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VersionedDictionaries",
                c => new
                    {
                        VersionedDictionaryID = c.Int(nullable: false, identity: true),
                        LanguageID = c.Int(nullable: false),
                        VersionName = c.String(),
                    })
                .PrimaryKey(t => t.VersionedDictionaryID);
            
            AddColumn("dbo.DictionaryEntries", "VersionedDictionaryID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DictionaryEntries", "VersionedDictionaryID");
            DropTable("dbo.VersionedDictionaries");
        }
    }
}
