using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201501061129)]
    public class M006_AddSystemUser : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Users").Row(new {Username = "admin", FullName = "System User", EmailAddress = "jsullivan@quoteflow.io", IsAdmin = 1, EmailAllowed = 1});

            Insert.IntoTable("Credentials")
                .Row(
                    new
                    {
                        UserId = 1,
                        Type = "password.pbkdf2",
                        Value = "AJDZnfZ8StYBwyQ6ktUwk9apWXCfMHbAIhohHWLKUSaF/N8DYfV47iNvG5i1EkBCHQ=="
                    });
        }

        public override void Down()
        {
            Delete.FromTable("Users").Row(new { Username = "admin" });
            Delete.FromTable("Credentials").Row(new { UserId = 1 });
        }
    }
}