namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormaPlatnosci : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Elements", "FormaPlatnosci", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Elements", "FormaPlatnosci");
        }
    }
}
