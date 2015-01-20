using FluentMigrator.Builders.Create.Table;

namespace LicenseManager.Infrastructure.Extensions
{
    internal static class FluentMigratorExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(this ICreateTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
        {
            return createTableColumnAsTypeSyntax.AsString(int.MaxValue);
        }
    }
}