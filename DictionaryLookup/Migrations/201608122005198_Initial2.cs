namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DictionaryErrorReports", "NGramEntryID", c => c.Long(nullable: false));
            DropColumn("dbo.DictionaryErrorReports", "VersionedDictionaryID");
            DropColumn("dbo.DictionaryErrorReports", "NGramStringID");
            DropColumn("dbo.DictionaryErrorReports", "NGramTagID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DictionaryErrorReports", "NGramTagID", c => c.Long(nullable: false));
            AddColumn("dbo.DictionaryErrorReports", "NGramStringID", c => c.Long(nullable: false));
            AddColumn("dbo.DictionaryErrorReports", "VersionedDictionaryID", c => c.Int(nullable: false));
            DropColumn("dbo.DictionaryErrorReports", "NGramEntryID");
        }
    }
}
