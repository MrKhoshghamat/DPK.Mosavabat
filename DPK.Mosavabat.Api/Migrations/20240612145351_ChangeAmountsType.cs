using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class ChangeAmountsType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c96cb7f8-7b0e-4b1d-a071-6bdf427ebc97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb370950-1426-4d9c-ac6d-668a12ed7df8");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrepaidAmount",
                table: "PaymentPermissions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ContractAmount",
                table: "PaymentPermissions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "03ead492-328f-41f1-8a84-0477e511bbaa", "a3ed6602-6d41-4d3f-aece-fbf59f848163", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "facb5919-6a90-48d7-9e52-a80a3926193e", "8c51a536-b294-49d9-8a0e-7b8ecefc9bec", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03ead492-328f-41f1-8a84-0477e511bbaa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "facb5919-6a90-48d7-9e52-a80a3926193e");

            migrationBuilder.AlterColumn<string>(
                name: "PrepaidAmount",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "ContractAmount",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c96cb7f8-7b0e-4b1d-a071-6bdf427ebc97", "80bf8c40-f278-4692-ae66-87cfd25da7f2", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fb370950-1426-4d9c-ac6d-668a12ed7df8", "0fa5cf48-61c9-422e-8f3c-7c4ef752d8d4", "Administrator", "ADMINISTRATOR" });
        }
    }
}
