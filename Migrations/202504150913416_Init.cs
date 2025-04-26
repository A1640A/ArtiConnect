namespace ArtiConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Endpoint = c.String(maxLength: 2147483647),
                        Method = c.String(maxLength: 2147483647),
                        RequestData = c.String(maxLength: 2147483647),
                        ResponseData = c.String(maxLength: 2147483647),
                        StatusCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ayars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Port = c.String(maxLength: 2147483647),
                        RemoteDbServerName = c.String(maxLength: 2147483647),
                        RemoteDbUserName = c.String(maxLength: 2147483647),
                        RemoteDbPassword = c.String(maxLength: 2147483647),
                        RemoteDbDatabaseName = c.String(maxLength: 2147483647),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ayars");
            DropTable("dbo.ApiLogs");
        }
    }
}
