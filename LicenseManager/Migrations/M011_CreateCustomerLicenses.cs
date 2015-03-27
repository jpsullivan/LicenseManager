using FluentMigrator;
using LicenseManager.Infrastructure.Extensions;

namespace LicenseManager.Migrations
{
    [Migration(201501061622)]
    public class M011_CreateCustomerLicenses : Migration
    {
        private const string TableName = "CustomerLicenses";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("CustomerId").AsInt32().NotNullable()
                .WithColumn("ProductId").AsInt32().NotNullable()
                .WithColumn("ServerId").AsString(18).NotNullable()
                .WithColumn("LicenseKey").AsMaxString().NotNullable()
                .WithColumn("ExpirationUtc").AsDateTime().Nullable()
                .WithColumn("CreatorId").AsInt32().Nullable()
                .WithColumn("CreatedUtc").AsDateTime().Nullable();

            Create.ForeignKey("FK_CustomerLicenses_Customers_CustomerId")
                .FromTable(TableName).ForeignColumn("CustomerId")
                .ToTable("Customers").PrimaryColumn("Id");

            Create.ForeignKey("FK_CustomerLicenses_Products_ProductId")
                .FromTable(TableName).ForeignColumn("ProductId")
                .ToTable("Products").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_CustomerLicenses_Customers_CustomerId").OnTable(TableName);
            Delete.ForeignKey("FK_CustomerLicenses_Products_ProductId").OnTable(TableName);
            Delete.Table(TableName);
        }
    }
}