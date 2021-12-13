namespace Infrastrucutre.Core.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemMasters",
                c => new
                    {
                        ItemMasterID = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemCode = c.String(),
                        Description = c.String(),
                        Brand = c.String(),
                        Dimension = c.String(),
                        ItemWeight = c.String(),
                        VAT = c.String(),
                        TotalCost = c.String(),
                        BarCode = c.String(),
                    })
                .PrimaryKey(t => t.ItemMasterID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ItemMasters");
        }
    }
}
