namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial11 : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.NGramEntries", "NGramStringID", c => c.Long(nullable: false));
            DropColumn("dbo.NGramEntries", "WordID");
            DropColumn("dbo.NGramEntries", "Previous1WordID");
            DropColumn("dbo.NGramEntries", "Previous2WordID");
            DropColumn("dbo.NGramEntries", "NGram");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NGramEntries", "NGram", c => c.Int(nullable: false));
            AddColumn("dbo.NGramEntries", "Previous2WordID", c => c.Long(nullable: false));
            AddColumn("dbo.NGramEntries", "Previous1WordID", c => c.Long(nullable: false));
            AddColumn("dbo.NGramEntries", "WordID", c => c.Long(nullable: false));
            DropColumn("dbo.NGramEntries", "NGramStringID");
            DropTable("dbo.NGramStrings");
        }
    }
}
