using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201502190034)]
    public class M015_AddCreatedUtcColumnToProductVersions : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductVersions").AddColumn("CreatedUtc").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CreatedUtc").FromTable("Products");
        }
    }
}