namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DictionaryWords", "TextPredictionCost", c => c.Byte(nullable: false));
            DropColumn("dbo.DictionaryWords", "Self");
        }

        public override void Down()
        {
            AlterColumn("dbo.DictionaryWords", "TextPredictionCost", c => c.Short(nullable: false));
            AddColumn("dbo.DictionaryWords", "Self", c => c.String(nullable: false));
        }
    }
}
