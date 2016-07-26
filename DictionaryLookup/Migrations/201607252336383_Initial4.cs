namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DictionaryWords", "HWRCost", c => c.Short(nullable: false));
            DropColumn("dbo.DictionaryWords", "HWRCalligScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DictionaryWords", "HWRCalligScore", c => c.Short(nullable: false));
            DropColumn("dbo.DictionaryWords", "HWRCost");
        }
    }
}
