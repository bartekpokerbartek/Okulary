namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormaPlatnosciWBinocle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Binocles", "FormaPlatnosci", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Binocles", "FormaPlatnosci");
        }
    }
}
