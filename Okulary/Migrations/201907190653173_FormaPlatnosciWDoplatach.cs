namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormaPlatnosciWDoplatach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doplatas", "FormaPlatnosci", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doplatas", "FormaPlatnosci");
        }
    }
}
