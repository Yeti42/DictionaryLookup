namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DictionaryErrorReports",
                c => new
                    {
                        DictionaryErrorReportId = c.Int(nullable: false, identity: true),
                        VersionedDictionaryID = c.Int(nullable: false),
                        NGramStringID = c.Long(nullable: false),
                        NGramTagID = c.Long(nullable: false),
                        ReportDateTime = c.DateTime(nullable: false),
                        ErrorTypeID = c.Short(nullable: false),
                        UserID = c.Int(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.DictionaryErrorReportId);
            
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
                "dbo.NGramStrings",
                c => new
                    {
                        NGramStringID = c.Long(nullable: false, identity: true),
                        WordID = c.Long(nullable: false),
                        Previous1WordID = c.Long(nullable: false),
                        Previous2WordID = c.Long(nullable: false),
                        NGram = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NGramStringID);
            
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserContact = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.VersionedDictionaries",
                c => new
                    {
                        VersionedDictionaryID = c.Int(nullable: false, identity: true),
                        LanguageID = c.Int(nullable: false),
                        VersionName = c.String(),
                    })
                .PrimaryKey(t => t.VersionedDictionaryID);
            
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
            DropTable("dbo.VersionedDictionaries");
            DropTable("dbo.Users");
            DropTable("dbo.NGramTags");
            DropTable("dbo.NGramStrings");
            DropTable("dbo.Languages");
            DropTable("dbo.ErrorTypes");
            DropTable("dbo.DictionaryErrorReports");
        }
    }
}
