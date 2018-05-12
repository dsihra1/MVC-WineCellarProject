namespace username_Wines.DAL.WineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Audi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wine", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.Wine", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Wine", "UpdatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.Wine", "UpdatedOn", c => c.DateTime());
            AddColumn("dbo.Wine", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Wine", "RowVersion");
            DropColumn("dbo.Wine", "UpdatedOn");
            DropColumn("dbo.Wine", "UpdatedBy");
            DropColumn("dbo.Wine", "CreatedOn");
            DropColumn("dbo.Wine", "CreatedBy");
        }
    }
}
