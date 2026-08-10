using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class AddCreationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ef62565-1b74-433b-ae8e-37524d6543c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c18baeb9-9bda-4e45-85fc-906c492d8858");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "PaymentPermissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "627cd1eb-c643-4c4e-b63b-fb9bfba1ca9e", "dc074249-691b-4c82-99e1-178e96c013a4", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "989c0bdd-35eb-41bc-b110-4cfda2d4b464", "1b0677a2-9971-4df2-9820-a62d27b468e8", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "627cd1eb-c643-4c4e-b63b-fb9bfba1ca9e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "989c0bdd-35eb-41bc-b110-4cfda2d4b464");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "PaymentPermissions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7ef62565-1b74-433b-ae8e-37524d6543c3", "1a6e22c1-673b-47b1-8e80-8ff66baa9895", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c18baeb9-9bda-4e45-85fc-906c492d8858", "bc74dd27-ca2f-48e3-9ade-0032de4862fd", "Manager", "MANAGER" });
        }
    }
}
