﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DealNotifier.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
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
                    Keyword = table.Column<string>(type: "nvarchar(50)", nullable: false),
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
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(15)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(15)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(5)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
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
                    Name = table.Column<string>(type: "nvarchar(15)", nullable: false),
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
                    Comment = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModelName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(15)", nullable: false),
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
                    Keywords = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidCount = table.Column<int>(type: "Int", nullable: false),
                    ConditionId = table.Column<int>(type: "int", nullable: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Image = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    IsAuction = table.Column<bool>(type: "BIT", nullable: false, defaultValueSql: "0"),
                    ItemEndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(254)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notify = table.Column<bool>(type: "BIT", nullable: false, defaultValueSql: "1"),
                    OldPrice = table.Column<decimal>(type: "decimal(13,2)", nullable: false, defaultValueSql: "0"),
                    OnlineStoreId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(13,2)", nullable: false),
                    Saving = table.Column<decimal>(type: "decimal(13,2)", nullable: false, defaultValueSql: "0"),
                    SavingsPercentage = table.Column<decimal>(type: "decimal(13,2)", nullable: false, defaultValueSql: "0"),
                    StockStatusId = table.Column<int>(type: "int", nullable: false),
                    UnlockProbabilityId = table.Column<int>(type: "int", nullable: true),
                    UnlockabledPhoneId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "default"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Condition",
                        column: x => x.ConditionId,
                        principalTable: "Condition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_ItemType",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_OnlineStore",
                        column: x => x.OnlineStoreId,
                        principalTable: "OnlineStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_StockStatus",
                        column: x => x.StockStatusId,
                        principalTable: "StockStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_UnlockProbability",
                        column: x => x.UnlockProbabilityId,
                        principalTable: "UnlockProbability",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_UnlockabledPhone",
                        column: x => x.UnlockabledPhoneId,
                        principalTable: "UnlockabledPhone",
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
                name: "UnlockabledPhonePhoneUnlockTool",
                columns: table => new
                {
                    UnlockabledPhoneId = table.Column<int>(type: "int", nullable: false),
                    PhoneUnlockToolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnlockabledPhonePhoneUnlockTool", x => new { x.PhoneUnlockToolId, x.UnlockabledPhoneId });
                    table.ForeignKey(
                        name: "FK_UnlockabledPhonePhoneUnlockTool_PhoneUnlockTool_PhoneUnlockToolId",
                        column: x => x.PhoneUnlockToolId,
                        principalTable: "PhoneUnlockTool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnlockabledPhonePhoneUnlockTool_UnlockabledPhone_UnlockabledPhoneId",
                        column: x => x.UnlockabledPhoneId,
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
                    { 20, null, null, "Google Fi", "GFI" },
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
                name: "IX_Item_ConditionId",
                table: "Item",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemTypeId",
                table: "Item",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Link",
                table: "Item",
                column: "Link",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_OnlineStoreId",
                table: "Item",
                column: "OnlineStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_PublicId",
                table: "Item",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_StockStatusId",
                table: "Item",
                column: "StockStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_UnlockabledPhoneId",
                table: "Item",
                column: "UnlockabledPhoneId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UnlockabledPhonePhoneUnlockTool_UnlockabledPhoneId",
                table: "UnlockabledPhonePhoneUnlockTool",
                column: "UnlockabledPhoneId");
        }

        /// <inheritdoc />
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
                name: "UnlockabledPhonePhoneUnlockTool");

            migrationBuilder.DropTable(
                name: "ItemType");

            migrationBuilder.DropTable(
                name: "OnlineStore");

            migrationBuilder.DropTable(
                name: "StockStatus");

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
