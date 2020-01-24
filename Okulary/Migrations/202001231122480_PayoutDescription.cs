namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayoutDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payouts", "Description", c => c.String(defaultValue: string.Empty));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payouts", "Description");
        }
    }
}
