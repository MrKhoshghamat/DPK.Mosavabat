using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class AddNewFieldsToPeymentPermissionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14dddfb2-4563-465e-ae33-d8305778edc2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7e7ad48-bc37-4931-bde5-e8286ebbda21");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalNo",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContractAmount",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrepaidAmount",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c96cb7f8-7b0e-4b1d-a071-6bdf427ebc97", "80bf8c40-f278-4692-ae66-87cfd25da7f2", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fb370950-1426-4d9c-ac6d-668a12ed7df8", "0fa5cf48-61c9-422e-8f3c-7c4ef752d8d4", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c96cb7f8-7b0e-4b1d-a071-6bdf427ebc97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb370950-1426-4d9c-ac6d-668a12ed7df8");

            migrationBuilder.DropColumn(
                name: "ApprovalNo",
                table: "PaymentPermissions");

            migrationBuilder.DropColumn(
                name: "ContractAmount",
                table: "PaymentPermissions");

            migrationBuilder.DropColumn(
                name: "PrepaidAmount",
                table: "PaymentPermissions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "14dddfb2-4563-465e-ae33-d8305778edc2", "b7ddeae7-768d-49b4-b98f-de924ecb1674", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d7e7ad48-bc37-4931-bde5-e8286ebbda21", "70446752-2fde-4e96-a57d-b2ff8c9f776a", "Manager", "MANAGER" });
        }
    }
}
