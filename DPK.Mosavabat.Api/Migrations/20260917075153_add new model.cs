using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class addnewmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "DateUpdate",
                table: "LoanBillRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeUpdate",
                table: "LoanBillRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DateUpdate",
                table: "LoanBillDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeUpdate",
                table: "LoanBillDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LCP_RequestFacilitySubmission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceSystem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProcessType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LcpId = table.Column<long>(type: "bigint", nullable: false),
                    RequestNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayloadHash = table.Column<byte[]>(type: "varbinary(32)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    OwnerToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaseUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttemptCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    RadNumber = table.Column<int>(type: "int", nullable: true),
                    LastErrorCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    SendingAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BizagiCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LCP_RequestFacilitySubmission", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07da4602-0948-40da-8815-124d7962d09a", "c90dd17c-f19a-4763-9672-f8e46702b0fb", "LoanManagementSystemAdmin", "LOANMANAGEMENTSYSTEMADMIN" },
                    { "746463e5-d871-43f1-b2ad-1b920357f685", "53a51e10-5b86-4750-b380-e42959260a49", "MosavabatAdmin", "MOSAVABATADMIN" },
                    { "8b56cb03-018c-4263-a067-fc97505d5cd9", "4320cbed-5035-4de6-b49c-cf3caa917060", "DpkAdmin", "DPKADMIN" },
                    { "d31185dc-c4ee-4925-8abf-908111d9af0e", "8469daef-694c-4fcd-a2ce-714e17dfbbb9", "BizagiAdmin", "BIZAGIADMIN" },
                    { "f268ab94-029a-4e2b-8c1b-019216ff702a", "6b1d9c9e-b9d7-4883-9135-d0c44ea3553a", "LegalCustomerPortalAdmin", "LEGALCUSTOMERPORTALADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LcpRequestFacilitySubmission_Recovery",
                table: "LCP_RequestFacilitySubmission",
                columns: new[] { "Status", "LeaseUntil" });

            migrationBuilder.CreateIndex(
                name: "UX_LcpRequestFacilitySubmission_BusinessKey",
                table: "LCP_RequestFacilitySubmission",
                columns: new[] { "SourceSystem", "ProcessType", "LcpId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LCP_RequestFacilitySubmission");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07da4602-0948-40da-8815-124d7962d09a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "746463e5-d871-43f1-b2ad-1b920357f685");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b56cb03-018c-4263-a067-fc97505d5cd9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d31185dc-c4ee-4925-8abf-908111d9af0e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f268ab94-029a-4e2b-8c1b-019216ff702a");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "LoanBillRequests");

            migrationBuilder.DropColumn(
                name: "TimeUpdate",
                table: "LoanBillRequests");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "LoanBillDatas");

            migrationBuilder.DropColumn(
                name: "TimeUpdate",
                table: "LoanBillDatas");

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
    }
}
