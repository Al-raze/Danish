namespace Infrastrucutre.Core.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Category_FK_item_category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemCategories",
                c => new
                    {
                        ItemCategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.ItemCategoryID);
            
            AddColumn("dbo.ItemMasters", "ItemCategoryID", c => c.Int(nullable: false));
            AddForeignKey("dbo.ItemMasters", "ItemCategoryID", "dbo.ItemCategories", "ItemCategoryID", cascadeDelete: true);
            CreateIndex("dbo.ItemMasters", "ItemCategoryID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ItemMasters", new[] { "ItemCategoryID" });
            DropForeignKey("dbo.ItemMasters", "ItemCategoryID", "dbo.ItemCategories");
            DropColumn("dbo.ItemMasters", "ItemCategoryID");
            DropTable("dbo.ItemCategories");
        }
    }
}
