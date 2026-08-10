using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class UpdatePaymentPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55c05e6c-f9dc-4964-aa7e-b9d0895f31e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71dcc7cd-9555-41ab-9f67-d736d553ddb9");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PaymentPermissions");

            migrationBuilder.AddColumn<bool>(
                name: "IsCancel",
                table: "PaymentPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "PaymentPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7ef62565-1b74-433b-ae8e-37524d6543c3", "1a6e22c1-673b-47b1-8e80-8ff66baa9895", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c18baeb9-9bda-4e45-85fc-906c492d8858", "bc74dd27-ca2f-48e3-9ade-0032de4862fd", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ef62565-1b74-433b-ae8e-37524d6543c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c18baeb9-9bda-4e45-85fc-906c492d8858");

            migrationBuilder.DropColumn(
                name: "IsCancel",
                table: "PaymentPermissions");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "PaymentPermissions");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PaymentPermissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "55c05e6c-f9dc-4964-aa7e-b9d0895f31e7", "5e95ded7-d1b7-44bf-a264-16d1ff2df5c1", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "71dcc7cd-9555-41ab-9f67-d736d553ddb9", "3dda70eb-fe21-4f69-a47a-9a8a23462952", "Manager", "MANAGER" });
        }
    }
}
