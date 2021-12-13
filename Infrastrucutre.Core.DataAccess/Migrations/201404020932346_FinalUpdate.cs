namespace Infrastrucutre.Core.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemManufacturers",
                c => new
                    {
                        ItemManufacturerID = c.Int(nullable: false, identity: true),
                        ManufacturerName = c.String(),
                    })
                .PrimaryKey(t => t.ItemManufacturerID);
            
            CreateTable(
                "dbo.ItemColors",
                c => new
                    {
                        ItemColorID = c.Int(nullable: false, identity: true),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.ItemColorID);
            
            AddColumn("dbo.ItemMasters", "ItemManufacturerID", c => c.Int(nullable: false));
            AddColumn("dbo.ItemMasters", "ItemColorID", c => c.Int(nullable: false));
            AddForeignKey("dbo.ItemMasters", "ItemManufacturerID", "dbo.ItemManufacturers", "ItemManufacturerID", cascadeDelete: true);
            AddForeignKey("dbo.ItemMasters", "ItemColorID", "dbo.ItemColors", "ItemColorID", cascadeDelete: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemMasters", "ItemColorID", "dbo.ItemColors");
            DropForeignKey("dbo.ItemMasters", "ItemManufacturerID", "dbo.ItemManufacturers");
            DropColumn("dbo.ItemMasters", "ItemColorID");
            DropColumn("dbo.ItemMasters", "ItemManufacturerID");
            DropTable("dbo.ItemColors");
            DropTable("dbo.ItemManufacturers");
        }
    }
}
