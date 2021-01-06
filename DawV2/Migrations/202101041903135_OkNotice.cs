namespace DawV2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OkNotice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notices", "Seen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notices", "Seen");
        }
    }
}
