using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201501061230)]
    public class M007_AddRoles : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Roles")
                .Row(new {Name = "Admins"})
                .Row(new {Name = "Managers"})
                .Row(new {Name = "Sales"})
                .Row(new {Name = "TechSupport"});
        }

        public override void Down()
        {
            Delete.FromTable("Roles").AllRows();
        }
    }
}