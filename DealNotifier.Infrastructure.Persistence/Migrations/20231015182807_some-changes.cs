using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealNotifier.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class somechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Brand_BrandId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_PhoneCarrier_PhoneCarrierId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_BrandId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_PhoneCarrierId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "PhoneCarrierId",
                table: "Item");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Item",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneCarrierId",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Item_BrandId",
                table: "Item",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_PhoneCarrierId",
                table: "Item",
                column: "PhoneCarrierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Brand_BrandId",
                table: "Item",
                column: "BrandId",
                principalTable: "Brand",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_PhoneCarrier_PhoneCarrierId",
                table: "Item",
                column: "PhoneCarrierId",
                principalTable: "PhoneCarrier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
