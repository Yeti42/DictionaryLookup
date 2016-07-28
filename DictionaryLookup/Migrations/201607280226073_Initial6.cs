namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DictionaryReports",
                c => new
                    {
                        DictionaryReportId = c.Int(nullable: false, identity: true),
                        WordID = c.Long(nullable: false),
                        LanguageID = c.Short(nullable: false),
                        DictionaryVersion = c.Short(nullable: false),
                        ReportDateTime = c.DateTime(nullable: false),
                        ErrorTypeID = c.Short(nullable: false),
                        UserID = c.Int(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.DictionaryReportId);
            
            CreateTable(
                "dbo.ErrorTypes",
                c => new
                    {
                        ErrorTypeID = c.Int(nullable: false, identity: true),
                        ErrorTypeName = c.String(),
                    })
                .PrimaryKey(t => t.ErrorTypeID);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguagesID = c.Short(nullable: false, identity: true),
                        BCP47 = c.String(),
                        FriendlyName = c.String(),
                    })
                .PrimaryKey(t => t.LanguagesID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserContact = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Languages");
            DropTable("dbo.ErrorTypes");
            DropTable("dbo.DictionaryReports");
        }
    }
}
