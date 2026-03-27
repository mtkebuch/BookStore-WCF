namespace BookStoreService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDateToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "PublishedDate", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "PublishedDate", c => c.DateTime(nullable: false));
        }
    }
}
