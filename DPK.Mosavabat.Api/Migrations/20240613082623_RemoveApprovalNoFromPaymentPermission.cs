using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class RemoveApprovalNoFromPaymentPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29d7d5c1-9df6-4e7b-a854-40b623273eaa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75cb4ead-4fe6-4858-887c-e474bbcc9f19");

            migrationBuilder.DropColumn(
                name: "ApprovalNo",
                table: "PaymentPermissions");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "84d47b70-74a5-417e-b099-a9cbe8a60066", "4251bd4e-8459-4645-9cf7-c282453f0be8", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9082758e-67c8-4b67-a334-e959769b233f", "8f91a342-5b2e-4bd7-aed5-6240651644d4", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84d47b70-74a5-417e-b099-a9cbe8a60066");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9082758e-67c8-4b67-a334-e959769b233f");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalNo",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "29d7d5c1-9df6-4e7b-a854-40b623273eaa", "45d9a307-89cd-4336-927c-1241603e2354", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "75cb4ead-4fe6-4858-887c-e474bbcc9f19", "3e4640ca-fd9a-434f-bc78-589445495bf3", "Manager", "MANAGER" });
        }
    }
}
