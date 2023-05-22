﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScraping.Infrastructure.Persistence.Migrations
{
    public partial class phonecarrierenumsmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "Item",
                type: "int",
                nullable: true,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "1");
        }
    }
}
