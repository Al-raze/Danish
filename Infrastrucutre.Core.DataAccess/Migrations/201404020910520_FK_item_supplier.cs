namespace Infrastrucutre.Core.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FK_item_supplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemMasters", "SupplierID", c => c.Int(nullable: false));
            AddForeignKey("dbo.ItemMasters", "SupplierID", "dbo.Suppliers", "SupplierID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemMasters", "SupplierID", "dbo.Suppliers");
            DropColumn("dbo.ItemMasters", "SupplierID");
        }
    }
}
