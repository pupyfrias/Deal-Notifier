using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealNotifier.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SomeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Keywords",
                table: "NotificationCriteria",
                newName: "IncludeKeywords");

            migrationBuilder.AddColumn<string>(
                name: "ExcludeKeywords",
                table: "NotificationCriteria",
                type: "nvarchar(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BanKeyword_Keyword",
                table: "BanKeyword",
                column: "Keyword",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BanKeyword_Keyword",
                table: "BanKeyword");

            migrationBuilder.DropColumn(
                name: "ExcludeKeywords",
                table: "NotificationCriteria");

            migrationBuilder.RenameColumn(
                name: "IncludeKeywords",
                table: "NotificationCriteria",
                newName: "Keywords");
        }
    }
}
