using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201501061620)]
    public class M010_CreateProducts : Migration
    {
        private const string TableName = "Products";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(256).NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}