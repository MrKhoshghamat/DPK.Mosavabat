using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class Add_LoanBill_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09e5f684-d703-4c6d-a4e8-7e0fbfbda27d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b409ac3d-918c-438e-8a57-53e339900094");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfc17d7e-847d-468f-ba43-cfaa4247de6b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c7301c64-989a-4f30-9a6b-7ccb8a55a24e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e82f28ee-2a70-4ad1-a091-a8ba08ac9282");

            migrationBuilder.CreateTable(
                name: "LoanBillDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanBillDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanBillRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsProcessing = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextRetryAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanBillRequests", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44633b7c-2584-45b7-87af-3e858b99c5d9", "b4619340-f29c-4e58-a9b4-49d999a3c700", "DpkAdmin", "DPKADMIN" },
                    { "a4dad091-ffd0-47ad-8e4c-25c549027dbc", "84d7120f-a41a-48dc-a11b-e23c8c662af7", "LoanManagementSystemAdmin", "LOANMANAGEMENTSYSTEMADMIN" },
                    { "e62c32b3-e13f-4c87-ae13-dd6946f8d411", "2b85c9c4-75aa-4b46-bea4-4b634df68d84", "LegalCustomerPortalAdmin", "LEGALCUSTOMERPORTALADMIN" },
                    { "f6c96609-1105-4b72-a4f5-205841960767", "cd981210-05d0-41b8-b80a-97d4b10088e1", "BizagiAdmin", "BIZAGIADMIN" },
                    { "fa1d21ee-5062-4b77-b52c-ab1bacec8256", "c5e23912-9b20-4c43-82f4-e99bea60b6eb", "MosavabatAdmin", "MOSAVABATADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanBillDatas");

            migrationBuilder.DropTable(
                name: "LoanBillRequests");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44633b7c-2584-45b7-87af-3e858b99c5d9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4dad091-ffd0-47ad-8e4c-25c549027dbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e62c32b3-e13f-4c87-ae13-dd6946f8d411");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6c96609-1105-4b72-a4f5-205841960767");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa1d21ee-5062-4b77-b52c-ab1bacec8256");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09e5f684-d703-4c6d-a4e8-7e0fbfbda27d", "8f6dd698-e06c-4c86-9fa1-ddc9b2e58af9", "BizagiAdmin", "BIZAGIADMIN" },
                    { "b409ac3d-918c-438e-8a57-53e339900094", "86d67191-b068-4c77-ba3f-b54ba015a9f4", "LegalCustomerPortalAdmin", "LEGALCUSTOMERPORTALADMIN" },
                    { "bfc17d7e-847d-468f-ba43-cfaa4247de6b", "3cb65573-c916-4d51-a698-f82044f3bb88", "MosavabatAdmin", "MOSAVABATADMIN" },
                    { "c7301c64-989a-4f30-9a6b-7ccb8a55a24e", "6258a40d-2feb-4cac-b912-e3584fe7e8a9", "DpkAdmin", "DPKADMIN" },
                    { "e82f28ee-2a70-4ad1-a091-a8ba08ac9282", "c5ab94c1-1822-4d78-8c96-c8e1cb266d74", "LoanManagementSystemAdmin", "LOANMANAGEMENTSYSTEMADMIN" }
                });
        }
    }
}
