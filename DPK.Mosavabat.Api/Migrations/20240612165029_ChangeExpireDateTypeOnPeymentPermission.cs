using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class ChangeExpireDateTypeOnPeymentPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03ead492-328f-41f1-8a84-0477e511bbaa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "facb5919-6a90-48d7-9e52-a80a3926193e");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpireDate",
                table: "PaymentPermissions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "29d7d5c1-9df6-4e7b-a854-40b623273eaa", "45d9a307-89cd-4336-927c-1241603e2354", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "75cb4ead-4fe6-4858-887c-e474bbcc9f19", "3e4640ca-fd9a-434f-bc78-589445495bf3", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29d7d5c1-9df6-4e7b-a854-40b623273eaa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75cb4ead-4fe6-4858-887c-e474bbcc9f19");

            migrationBuilder.AlterColumn<string>(
                name: "ExpireDate",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "03ead492-328f-41f1-8a84-0477e511bbaa", "a3ed6602-6d41-4d3f-aece-fbf59f848163", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "facb5919-6a90-48d7-9e52-a80a3926193e", "8c51a536-b294-49d9-8a0e-7b8ecefc9bec", "Administrator", "ADMINISTRATOR" });
        }
    }
}
