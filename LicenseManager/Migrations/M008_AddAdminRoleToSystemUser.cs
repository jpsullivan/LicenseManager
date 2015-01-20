using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201501061238)]
    public class M008_AddAdminRoleToSystemUser : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("UserRoles").Row(new {UserId = 1, RoleId = 1});
        }

        public override void Down()
        {
            Delete.FromTable("UserRoles").Row(new {UserId = 1});
        }
    }
}