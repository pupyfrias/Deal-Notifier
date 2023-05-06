using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScraping.Infrastructure.Persistence.Migrations
{
    public partial class uniquelinkadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Item_Link",
                table: "Item",
                column: "Link",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Item_Link",
                table: "Item");
        }
    }
}