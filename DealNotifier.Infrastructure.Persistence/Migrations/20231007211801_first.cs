using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealNotifier.Infrastructure.Persistence.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BanKeyword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keyword = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanKeyword", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BanLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "VARCHAR(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanLink", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(15)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlineStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(15)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineStore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneCarrier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    ShortName = table.Column<string>(type: "VARCHAR(5)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneCarrier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneUnlockTool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneUnlockTool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnlockProbability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(15)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlockProbability", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnlockabledPhone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    ModelName = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    ModelNumber = table.Column<string>(type: "VARCHAR(15)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlockabledPhone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnlockabledPhone_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keywords = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "DECIMAL(13,2)", nullable: false),
                    ConditionId = table.Column<int>(type: "INT", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConditionsToModify_Condition",
                        column: x => x.ConditionId,
                        principalTable: "Condition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    OldPrice = table.Column<decimal>(type: "DECIMAL(13,2)", nullable: false, defaultValueSql: "0"),
                    Price = table.Column<decimal>(type: "DECIMAL(13,2)", nullable: false),
                    Saving = table.Column<decimal>(type: "DECIMAL(13,2)", nullable: false, defaultValueSql: "0"),
                    SavingsPercentage = table.Column<decimal>(type: "DECIMAL(13,2)", nullable: false, defaultValueSql: "0"),
                    ConditionId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(max)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    PhoneCarrierId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "1"),
                    ModelNumber = table.Column<string>(type: "varchar(25)", nullable: true),
                    ModelName = table.Column<string>(type: "varchar(25)", nullable: true),
                    Notify = table.Column<bool>(type: "BIT", nullable: false, defaultValueSql: "1"),
                    IsAuction = table.Column<bool>(type: "BIT", nullable: false, defaultValueSql: "0"),
                    BidCount = table.Column<int>(type: "Int", nullable: false),
                    UnlockProbabilityId = table.Column<int>(type: "int", nullable: false),
                    ItemEndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Notified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Brand",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_Condition",
                        column: x => x.ConditionId,
                        principalTable: "Condition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_PhoneCarrier",
                        column: x => x.PhoneCarrierId,
                        principalTable: "PhoneCarrier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_Shop",
                        column: x => x.ShopId,
                        principalTable: "OnlineStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_Status",
                        column: x => x.StatusId,
                        principalTable: "StockStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_Type",
                        column: x => x.TypeId,
                        principalTable: "ItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_UnlockProbability",
                        column: x => x.UnlockProbabilityId,
                        principalTable: "UnlockProbability",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnlockabledPhonePhoneCarrier",
                columns: table => new
                {
                    UnlockabledPhoneId = table.Column<int>(type: "int", nullable: false),
                    PhoneCarrierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlockabledPhonePhoneCarrier", x => new { x.UnlockabledPhoneId, x.PhoneCarrierId });
                    table.ForeignKey(
                        name: "FK_UnlockabledPhonePhoneCarrier_PhoneCarrier_PhoneCarrierId",
                        column: x => x.PhoneCarrierId,
                        principalTable: "PhoneCarrier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnlockabledPhonePhoneCarrier_UnlockabledPhone_UnlockabledPhoneId",
                        column: x => x.UnlockabledPhoneId,
                        principalTable: "UnlockabledPhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnlockabledPhoneUnlockTool",
                columns: table => new
                {
                    UnlockablePhoneId = table.Column<int>(type: "int", nullable: false),
                    PhoneUnlockToolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlockabledPhoneUnlockTool", x => new { x.PhoneUnlockToolId, x.UnlockablePhoneId });
                    table.ForeignKey(
                        name: "FK_UnlockabledPhoneUnlockTool_PhoneUnlockTool_PhoneUnlockToolId",
                        column: x => x.PhoneUnlockToolId,
                        principalTable: "PhoneUnlockTool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnlockabledPhoneUnlockTool_UnlockabledPhone_PhoneUnlockToolId",
                        column: x => x.PhoneUnlockToolId,
                        principalTable: "UnlockabledPhone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Unknown" },
                    { 2, null, null, "Samsung" },
                    { 3, null, null, "Apple" },
                    { 4, null, null, "Motorola" },
                    { 5, null, null, "LG" },
                    { 6, null, null, "Huawei" },
                    { 7, null, null, "Alcatel" },
                    { 8, null, null, "Xiaomi" },
                    { 9, null, null, "HTC" }
                });

            migrationBuilder.InsertData(
                table: "Condition",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "New" },
                    { 2, null, null, "Used" }
                });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Accessory" },
                    { 2, null, null, "Headphone" },
                    { 3, null, null, "Memory" },
                    { 4, null, null, "Microphone" },
                    { 5, null, null, "Phone" },
                    { 6, null, null, "Speaker" },
                    { 7, null, null, "Streaming" },
                    { 8, null, null, "TV" }
                });

            migrationBuilder.InsertData(
                table: "OnlineStore",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Amazon" },
                    { 2, null, null, "eBay" },
                    { 3, null, null, "TheStore" }
                });

            migrationBuilder.InsertData(
                table: "PhoneCarrier",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, null, null, "Unknown", "UNK" },
                    { 2, null, null, "All Carriers", "ALL" },
                    { 3, null, null, "AT&T|ATT", "ATT" },
                    { 4, null, null, "Verizon", "VZW" },
                    { 5, null, null, "T-Mobile", "TMB" },
                    { 6, null, null, "Sprint", "SPR" },
                    { 7, null, null, "U.S. Cellular", "USC" },
                    { 8, null, null, "CenturyLink", "CTL" },
                    { 9, null, null, "Spectrum", "CHA" },
                    { 10, null, null, "Xfinity", "XFN" },
                    { 11, null, null, "Cricket", "AIO" },
                    { 12, null, null, "Metro|MetroPCS", "TMK" },
                    { 13, null, null, "TracFone", "TFN" },
                    { 14, null, null, "Boost Mobile|Boost", "BST" },
                    { 15, null, null, "Q Link Wireless", "QLK" },
                    { 16, null, null, "Republic Wireless", "RPW" },
                    { 17, null, null, "Straight Talk", "STK" },
                    { 18, null, null, "Virgin Mobile|Virgin", "VMU" },
                    { 19, null, null, "Total Wireless", "TWL" },
                    { 20, null, null, "Google Fi", "GFI" }
                });

            migrationBuilder.InsertData(
                table: "PhoneCarrier",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name", "ShortName" },
                values: new object[,]
                {
                    { 21, null, null, "Mint Mobile|Mint", "MNT" },
                    { 22, null, null, "Ting", "TNG" },
                    { 23, null, null, "Consumer Cellular", "CCU" },
                    { 24, null, null, "Credo Mobile", "CRD" },
                    { 25, null, null, "FreedomPop", "FDM" },
                    { 26, null, null, "Net10 Wireless|Net10", "NTW" },
                    { 27, null, null, "Dish", "DSH" }
                });

            migrationBuilder.InsertData(
                table: "PhoneUnlockTool",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "T-Unlock" },
                    { 2, null, null, "SamKey" },
                    { 3, null, null, "Global Unlocker" }
                });

            migrationBuilder.InsertData(
                table: "StockStatus",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "InStock" },
                    { 2, null, null, "OutStock" }
                });

            migrationBuilder.InsertData(
                table: "UnlockProbability",
                columns: new[] { "Id", "LastModified", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "None" },
                    { 2, null, null, "Low" },
                    { 3, null, null, "Middle" },
                    { 4, null, null, "High" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_BrandId",
                table: "Item",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ConditionId",
                table: "Item",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Link",
                table: "Item",
                column: "Link",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_PhoneCarrierId",
                table: "Item",
                column: "PhoneCarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ShopId",
                table: "Item",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_StatusId",
                table: "Item",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_TypeId",
                table: "Item",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_UnlockProbabilityId",
                table: "Item",
                column: "UnlockProbabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCriteria_ConditionId",
                table: "NotificationCriteria",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneCarrier_ShortName",
                table: "PhoneCarrier",
                column: "ShortName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnlockabledPhone_BrandId",
                table: "UnlockabledPhone",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_UnlockabledPhone_ModelNumber",
                table: "UnlockabledPhone",
                column: "ModelNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnlockabledPhonePhoneCarrier_PhoneCarrierId",
                table: "UnlockabledPhonePhoneCarrier",
                column: "PhoneCarrierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BanKeyword");

            migrationBuilder.DropTable(
                name: "BanLink");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "NotificationCriteria");

            migrationBuilder.DropTable(
                name: "UnlockabledPhonePhoneCarrier");

            migrationBuilder.DropTable(
                name: "UnlockabledPhoneUnlockTool");

            migrationBuilder.DropTable(
                name: "OnlineStore");

            migrationBuilder.DropTable(
                name: "StockStatus");

            migrationBuilder.DropTable(
                name: "ItemType");

            migrationBuilder.DropTable(
                name: "UnlockProbability");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "PhoneCarrier");

            migrationBuilder.DropTable(
                name: "PhoneUnlockTool");

            migrationBuilder.DropTable(
                name: "UnlockabledPhone");

            migrationBuilder.DropTable(
                name: "Brand");
        }
    }
}
