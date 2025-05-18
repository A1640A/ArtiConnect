namespace ArtiConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ayars", "YemekSepetiCurrentToken", c => c.String(maxLength: 2147483647));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ayars", "YemekSepetiCurrentToken");
        }
    }
}
