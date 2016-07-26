namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DictionaryWords", "HWRCalligraphyCost", c => c.Short(nullable: false));
            DropColumn("dbo.DictionaryWords", "HWRWordCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DictionaryWords", "HWRWordCost", c => c.Short(nullable: false));
            DropColumn("dbo.DictionaryWords", "HWRCalligraphyCost");
        }
    }
}
