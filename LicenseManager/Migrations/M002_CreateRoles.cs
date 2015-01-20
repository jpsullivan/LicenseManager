using FluentMigrator;
using LicenseManager.Infrastructure.Extensions;

namespace LicenseManager.Migrations
{
    [Migration(201412021307)]
    public class M002_CreateRoles : Migration
    {
        private const string TableName = "Roles";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsMaxString().Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}