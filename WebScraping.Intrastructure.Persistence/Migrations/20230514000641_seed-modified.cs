using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScraping.Infrastructure.Persistence.Migrations
{
    public partial class seedmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "All Carriers", "ALL" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "AT&T", "ATT" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Verizon", "VZW" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "T-Mobile", "TMB" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Sprint", "SPR" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "U.S. Cellular", "USC" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "CenturyLink", "CTL" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Spectrum", "CHA" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Xfinity", "XFN" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Cricket", "AIO" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Metro", "TMK" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "TracFone", "TFN" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Boost Mobile", "BST" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Q Link Wireless", "QLK" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Republic Wireless", "RPW" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Straight Talk", "STK" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Virgin Mobile USA", "VMU" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Total Wireless", "TWL" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Google Fi", "GFI" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Mint Mobile", "MNT" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Ting", "TNG" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Consumer Cellular", "CCU" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Credo Mobile", "CRD" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "FreedomPop", "FDM" });

            migrationBuilder.InsertData(
                table: "PhoneCarrier",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name", "ShortName" },
                values: new object[] { 26, null, null, "Net10 Wireless", "NTW" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "AT&T", "ATT" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Verizon", "VZW" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "T-Mobile", "TMB" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Sprint", "SPR" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "U.S. Cellular", "USC" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "CenturyLink", "CTL" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Spectrum", "CHA" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Xfinity", "XFN" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Cricket", "AIO" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Metro", "TMK" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "TracFone", "TFN" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Boost Mobile", "BST" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Q Link Wireless", "QLK" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Republic Wireless", "RPW" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Straight Talk", "STK" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Virgin Mobile USA", "VMU" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Total Wireless", "TWL" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Google Fi", "GFI" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Mint Mobile", "MNT" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Ting", "TNG" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Consumer Cellular", "CCU" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Credo Mobile", "CRD" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "FreedomPop", "FDM" });

            migrationBuilder.UpdateData(
                table: "PhoneCarrier",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "ShortName" },
                values: new object[] { "Net10 Wireless", "NTW" });
        }
    }
}
