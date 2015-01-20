using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201412021313)]
    public class M005_CreateElmah : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("Elmah.SqlServer.sql");
        }

        public override void Down()
        {
            Delete.Table("ELMAH_Error");
        }
    }
}