namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.DictionaryWords");
            AddColumn("dbo.DictionaryWords", "NGram", c => c.Short(nullable: false));
            AlterColumn("dbo.DictionaryWords", "DictionaryWordID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.DictionaryWords", "DictionaryWordID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DictionaryWords");
            AlterColumn("dbo.DictionaryWords", "DictionaryWordID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.DictionaryWords", "NGram");
            AddPrimaryKey("dbo.DictionaryWords", "DictionaryWordID");
        }
    }
}
