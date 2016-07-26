namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DictionaryWords", "TextPredictionBackOffCost", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DictionaryWords", "TextPredictionBackOffCost", c => c.Short(nullable: false));
        }
    }
}
