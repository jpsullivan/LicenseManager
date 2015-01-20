using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201501061607)]
    public class M009_CreateCustomers : Migration
    {
        private const string TableName = "Customers";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(128).NotNullable()
                .WithColumn("BillingContact").AsString(100).Nullable()
                .WithColumn("BillingContactEmail").AsString(256).Nullable()
                .WithColumn("TechnicalContact").AsString(100).Nullable()
                .WithColumn("TechnicalContactEmail").AsString(256).Nullable()
                .WithColumn("IsHosted").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedUtc").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}