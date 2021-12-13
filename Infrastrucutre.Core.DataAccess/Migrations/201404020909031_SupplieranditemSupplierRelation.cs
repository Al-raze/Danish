namespace Infrastrucutre.Core.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplieranditemSupplierRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        SalesRepName = c.String(),
                        WebAddress = c.String(),
                        BankDetails = c.String(),
                    })
                .PrimaryKey(t => t.SupplierID);
        }
        
        public override void Down()
        {
            DropTable("dbo.Suppliers");
            
        }
    }
}
