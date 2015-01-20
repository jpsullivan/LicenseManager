using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201412021306)]
    public class M001_CreateUsers : Migration
    {
        private const string TableName = "Users";

        public override void Up()
        {
            Create.Table(TableName)
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Username").AsString(64).NotNullable()
                .WithColumn("EmailAddress").AsString(256).Nullable()
                .WithColumn("FullName").AsString(100).Nullable()
                .WithColumn("IsAdmin").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("EmailAllowed").AsBoolean().NotNullable()
                .WithColumn("PasswordResetToken").AsString(32).Nullable()
                .WithColumn("PasswordResetTokenExpirationDate").AsDateTime().Nullable()
                .WithColumn("CreatedUtc").AsDateTime().Nullable()
                .WithColumn("LastLoginUtc").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableName);
        }
    }
}