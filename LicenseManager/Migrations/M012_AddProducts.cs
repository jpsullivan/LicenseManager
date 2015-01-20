using FluentMigrator;

namespace LicenseManager.Migrations
{
    [Migration(201501121355)]
    public class M012_AddProducts : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Products")
                .Row(new {Name = "Intra Desktop"})
                .Row(new {Name = "Intra Mobile"})
                .Row(new {Name = "Intra Omni"});
        }

        public override void Down()
        {
            Delete.FromTable("Products").AllRows();
        }
    }
}