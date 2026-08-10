using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class ChangeSomePaymentPermissionFieldsToNullabe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "627cd1eb-c643-4c4e-b63b-fb9bfba1ca9e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "989c0bdd-35eb-41bc-b110-4cfda2d4b464");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrepaidAmount",
                table: "PaymentPermissions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "C13",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "101d4bd2-4342-4884-9a80-bc3468e938fb", "b0670cd5-c77e-46d2-a1dd-5e54ba3257e6", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2f176c76-dcb7-4b25-9726-37fb20902cd9", "8ea0bdd3-ea53-4985-9c17-53b61d058064", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "101d4bd2-4342-4884-9a80-bc3468e938fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f176c76-dcb7-4b25-9726-37fb20902cd9");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrepaidAmount",
                table: "PaymentPermissions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "C13",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "627cd1eb-c643-4c4e-b63b-fb9bfba1ca9e", "dc074249-691b-4c82-99e1-178e96c013a4", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "989c0bdd-35eb-41bc-b110-4cfda2d4b464", "1b0677a2-9971-4df2-9820-a62d27b468e8", "Administrator", "ADMINISTRATOR" });
        }
    }
}
