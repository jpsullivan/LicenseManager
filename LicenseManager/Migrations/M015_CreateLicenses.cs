using FluentMigrator;
using LicenseManager.Infrastructure.Extensions;

namespace LicenseManager.Migrations
{
    [Migration(201503271642)]
    public class M015_CreateLicenses : Migration
    {
        private const string TableName = "Licenses";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("CustomerId").AsInt32().NotNullable()
                .WithColumn("FeatureProperties").AsMaxString().Nullable()
                .WithColumn("MachineId").AsString(256).NotNullable()
                .WithColumn("ServerName").AsString(128).NotNullable()
                .WithColumn("DatabaseName").AsString(128).NotNullable()
                .WithColumn("CreatedBy").AsInt32().NotNullable()
                .WithColumn("CreatedUtc").AsDateTime().Nullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.ForeignKey("FK_Licenses_Customers_CustomerId")
                .FromTable(TableName).ForeignColumn("CustomerId")
                .ToTable("Customers").PrimaryColumn("Id");

            Create.ForeignKey("FK_Licenses_Users_CreatedBy")
                .FromTable(TableName).ForeignColumn("CreatedBy")
                .ToTable("Users").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Licenses_Users_CreatedBy").OnTable(TableName);
            Delete.ForeignKey("FK_Licenses_Customers_CustomerId").OnTable(TableName);
            Delete.Table(TableName);
        }
    }
}