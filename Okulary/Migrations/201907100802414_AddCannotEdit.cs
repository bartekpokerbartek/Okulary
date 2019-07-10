namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCannotEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Elements", "CannotEdit", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Elements", "CannotEdit");
        }
    }
}
