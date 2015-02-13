using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201502122220)]
    public class M013_AddMoreProductColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("Products")
                .AddColumn("Description").AsString().Nullable()
                .AddColumn("Url").AsString().Nullable()
                .AddColumn("CreatedBy").AsString().NotNullable().WithDefault(SystemMethods.CurrentUser)
                .AddColumn("CreatedUtc").AsDateTime().Nullable()
                .AddColumn("LastUpdatedUtc").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Description")
                .Column("Url")
                .Column("CreatedBy")
                .Column("CreatedUtc")
                .Column("LastUpdatedUtc")
                .FromTable("Products");

        }
    }
}