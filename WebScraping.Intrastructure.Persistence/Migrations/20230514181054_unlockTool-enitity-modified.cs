using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScraping.Infrastructure.Persistence.Migrations
{
    public partial class unlockToolenititymodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnlockTool",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[] { 1, null, null, "T-Unlock" });

            migrationBuilder.InsertData(
                table: "UnlockTool",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[] { 2, null, null, "SamKey" });

            migrationBuilder.InsertData(
                table: "UnlockTool",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[] { 3, null, null, "Global Unlocker" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnlockTool",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnlockTool",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnlockTool",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
