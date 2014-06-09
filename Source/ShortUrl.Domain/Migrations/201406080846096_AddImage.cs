namespace ShortUrl.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImage : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Urls",
            //    c => new
            //        {
            //            ShortenedUrl = c.String(nullable: false, maxLength: 128),
            //            RealUrl = c.String(),
            //            Date = c.DateTime(nullable: false),
            //            UserName = c.String(),
            //            //Image = c.Binary(),
            //        })
            //    .PrimaryKey(t => t.ShortenedUrl);

            AddColumn("dbo.Urls", "Image", c => c.Binary());
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Urls");
            DropColumn("dbo.Urls", "Image");
        }
    }
}
