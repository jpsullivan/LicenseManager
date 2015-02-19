using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201502190149)]
    public class M016_CreateProductFeatures : Migration
    {
        private const string TableName = "ProductFeatures";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("ProductId").AsInt32().NotNullable()
                .WithColumn("VersionId").AsInt32().NotNullable()
                .WithColumn("Name").AsString(256).NotNullable()
                .WithColumn("Key").AsString(512).NotNullable()
                .WithColumn("DataType").AsString(12).NotNullable()
                .WithColumn("DefaultValue").AsString(256).Nullable()
                .WithColumn("CreatedUtc").AsDateTime().Nullable();

            Create.ForeignKey("FK_ProductFeatures_ProductFeatures")
                .FromTable(TableName).ForeignColumn("Id")
                .ToTable(TableName).PrimaryColumn("Id");

            Create.ForeignKey("FK_ProductFeatures_Products_ProductId")
                .FromTable(TableName).ForeignColumn("ProductId")
                .ToTable("Products").PrimaryColumn("Id");

            Create.ForeignKey("FK_ProductFeatures_ProductVersions_VersionId")
                .FromTable(TableName).ForeignColumn("VersionId")
                .ToTable("ProductVersions").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_ProductFeatures_ProductVersions_VersionId");
            Delete.ForeignKey("FK_ProductFeatures_Products_ProductId");
            Delete.ForeignKey("FK_ProductFeatures_ProductFeatures");
            Delete.Table(TableName);
        }
    }
}