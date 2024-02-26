using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealNotifier.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ItemModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_UnlockProbability",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Item",
                newName: "Title");

            migrationBuilder.AlterColumn<int>(
                name: "UnlockProbabilityId",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Item",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_UnlockProbability",
                table: "Item",
                column: "UnlockProbabilityId",
                principalTable: "UnlockProbability",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_UnlockProbability",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Item",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "UnlockProbabilityId",
                table: "Item",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_UnlockProbability",
                table: "Item",
                column: "UnlockProbabilityId",
                principalTable: "UnlockProbability",
                principalColumn: "Id");
        }
    }
}
