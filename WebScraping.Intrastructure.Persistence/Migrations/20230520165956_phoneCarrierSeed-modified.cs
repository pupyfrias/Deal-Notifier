using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScraping.Infrastructure.Persistence.Migrations
{
    public partial class phoneCarrierSeedmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ModelName",
                table: "Unlockable",
                type: "VARCHAR(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(15)");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "AT&T|ATT");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Metro|MetroPCS");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Boost Mobile|Boost");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "Virgin Mobile|Virgin");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "Mint Mobile|Mint");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Net10 Wireless|Net10");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ModelName",
                table: "Unlockable",
                type: "VARCHAR(15)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "AT&T");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Metro");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Boost Mobile");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "Virgin Mobile USA");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "Mint Mobile");

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Net10 Wireless");
        }
    }
}
