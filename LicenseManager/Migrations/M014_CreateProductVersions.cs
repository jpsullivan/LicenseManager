using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201502131130)]
    public class M014_CreateProductVersions : Migration
    {
        private const string TableName = "ProductVersions";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("ProductId").AsInt32().NotNullable()
                .WithColumn("Version").AsString(64).NotNullable();

            Create.ForeignKey("FK_ProductVersions_ProductVersions")
                .FromTable(TableName).ForeignColumn("Id")
                .ToTable(TableName).PrimaryColumn("Id");

            Create.ForeignKey("FK_ProductVersions_Products_ProductId")
                .FromTable(TableName).ForeignColumn("ProductId")
                .ToTable("Products").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_ProductVersions_Products_ProductId");
            Delete.ForeignKey("FK_ProductVersions_ProductVersions");
            Delete.Table(TableName);
        }
    }
}