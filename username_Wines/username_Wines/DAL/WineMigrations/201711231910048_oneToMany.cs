namespace username_Wines.DAL.WineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oneToMany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wine", "Wine_TypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Wine", "Wine_TypeID");
            AddForeignKey("dbo.Wine", "Wine_TypeID", "dbo.Wine_Type", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wine", "Wine_TypeID", "dbo.Wine_Type");
            DropIndex("dbo.Wine", new[] { "Wine_TypeID" });
            DropColumn("dbo.Wine", "Wine_TypeID");
        }
    }
}
