namespace Okulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BinoclesExtended : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Binocles", "NumerZlecenia", c => c.String());
            AddColumn("dbo.Binocles", "DataOdbioru", c => c.DateTime(nullable: false));
            AddColumn("dbo.Binocles", "Zadatek", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "RodzajOprawekDal", c => c.String());
            AddColumn("dbo.Binocles", "CenaOprawekDal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "RodzajOprawekBliz", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "CenaOprawekBliz", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "Robocizna", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOP_Sfera", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOP_Cylinder", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOP_Os", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOP_Pryzma", c => c.String());
            AddColumn("dbo.Binocles", "DalOP_OdlegloscZrenic", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOP_H", c => c.String());
            AddColumn("dbo.Binocles", "DalOP_Cena", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOL_Sfera", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOL_Cylinder", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOL_Os", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOL_Pryzma", c => c.String());
            AddColumn("dbo.Binocles", "DalOL_OdlegloscZrenic", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "DalOL_H", c => c.String());
            AddColumn("dbo.Binocles", "DalOL_Cena", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOP_Sfera", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOP_Cylinder", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOP_Os", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOP_Pryzma", c => c.String());
            AddColumn("dbo.Binocles", "BlizOP_OdlegloscZrenic", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOP_H", c => c.String());
            AddColumn("dbo.Binocles", "BlizOP_Cena", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOL_Sfera", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOL_Cylinder", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOL_Os", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOL_Pryzma", c => c.String());
            AddColumn("dbo.Binocles", "BlizOL_OdlegloscZrenic", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "BlizOL_H", c => c.String());
            AddColumn("dbo.Binocles", "BlizOL_Cena", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Binocles", "RodzajSoczewek1", c => c.String());
            AddColumn("dbo.Binocles", "RodzajSoczewek2", c => c.String());
            AddColumn("dbo.Binocles", "Refundacja", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.People", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Address");
            DropColumn("dbo.Binocles", "Refundacja");
            DropColumn("dbo.Binocles", "RodzajSoczewek2");
            DropColumn("dbo.Binocles", "RodzajSoczewek1");
            DropColumn("dbo.Binocles", "BlizOL_Cena");
            DropColumn("dbo.Binocles", "BlizOL_H");
            DropColumn("dbo.Binocles", "BlizOL_OdlegloscZrenic");
            DropColumn("dbo.Binocles", "BlizOL_Pryzma");
            DropColumn("dbo.Binocles", "BlizOL_Os");
            DropColumn("dbo.Binocles", "BlizOL_Cylinder");
            DropColumn("dbo.Binocles", "BlizOL_Sfera");
            DropColumn("dbo.Binocles", "BlizOP_Cena");
            DropColumn("dbo.Binocles", "BlizOP_H");
            DropColumn("dbo.Binocles", "BlizOP_OdlegloscZrenic");
            DropColumn("dbo.Binocles", "BlizOP_Pryzma");
            DropColumn("dbo.Binocles", "BlizOP_Os");
            DropColumn("dbo.Binocles", "BlizOP_Cylinder");
            DropColumn("dbo.Binocles", "BlizOP_Sfera");
            DropColumn("dbo.Binocles", "DalOL_Cena");
            DropColumn("dbo.Binocles", "DalOL_H");
            DropColumn("dbo.Binocles", "DalOL_OdlegloscZrenic");
            DropColumn("dbo.Binocles", "DalOL_Pryzma");
            DropColumn("dbo.Binocles", "DalOL_Os");
            DropColumn("dbo.Binocles", "DalOL_Cylinder");
            DropColumn("dbo.Binocles", "DalOL_Sfera");
            DropColumn("dbo.Binocles", "DalOP_Cena");
            DropColumn("dbo.Binocles", "DalOP_H");
            DropColumn("dbo.Binocles", "DalOP_OdlegloscZrenic");
            DropColumn("dbo.Binocles", "DalOP_Pryzma");
            DropColumn("dbo.Binocles", "DalOP_Os");
            DropColumn("dbo.Binocles", "DalOP_Cylinder");
            DropColumn("dbo.Binocles", "DalOP_Sfera");
            DropColumn("dbo.Binocles", "Robocizna");
            DropColumn("dbo.Binocles", "CenaOprawekBliz");
            DropColumn("dbo.Binocles", "RodzajOprawekBliz");
            DropColumn("dbo.Binocles", "CenaOprawekDal");
            DropColumn("dbo.Binocles", "RodzajOprawekDal");
            DropColumn("dbo.Binocles", "Zadatek");
            DropColumn("dbo.Binocles", "DataOdbioru");
            DropColumn("dbo.Binocles", "NumerZlecenia");
        }
    }
}
