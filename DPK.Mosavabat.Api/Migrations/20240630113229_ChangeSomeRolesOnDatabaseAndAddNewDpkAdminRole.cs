using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class ChangeSomeRolesOnDatabaseAndAddNewDpkAdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03a0b56b-43fc-4ccc-9520-08b7b2686a07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ebae1ce-b55d-4193-90a5-9faea5c2b177");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6117596-6893-4163-b677-68cb0b6372c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f05f7f94-2986-4955-a91e-7c1482f31f7d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "368c70ee-ab49-4f43-8e04-44f1a69913bc", "17d041df-60a4-43bc-af1a-85908cfa008e", "BizagiAdmin", "BIZAGIADMIN" },
                    { "3d406264-fa86-4a61-9dd8-f80d3a71e4df", "c2a2925c-5a09-4e5c-841f-d735e688fa97", "MosavabatAdmin", "MOSAVABATADMIN" },
                    { "72327633-c26a-4b5b-b470-6d7936da4303", "b1a5d519-caa3-4229-a2ff-d36b5ca5b4ae", "LoanManagementSystemAdmin", "LOANMANAGEMENTSYSTEMADMIN" },
                    { "74e57ff7-58e1-468b-9004-ef546263aad8", "5dd493cd-90c4-4336-95ab-6affb575d1e0", "LegalCustomerPortalAdmin", "LEGALCUSTOMERPORTALADMIN" },
                    { "f29dc856-e8e8-4f9c-a89a-8f97c2d98a6a", "5322a765-22e5-4209-8f4a-4049bd4f8a6b", "DpkAdmin", "DPKADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "368c70ee-ab49-4f43-8e04-44f1a69913bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d406264-fa86-4a61-9dd8-f80d3a71e4df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72327633-c26a-4b5b-b470-6d7936da4303");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74e57ff7-58e1-468b-9004-ef546263aad8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f29dc856-e8e8-4f9c-a89a-8f97c2d98a6a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03a0b56b-43fc-4ccc-9520-08b7b2686a07", "314dd019-c5dd-4c08-a0de-b261e795561a", "LegalCustomerPortalAdmin", "LEGALCUSTOMERPORTALADMIN" },
                    { "5ebae1ce-b55d-4193-90a5-9faea5c2b177", "70f296b2-00ea-40f5-927c-5b1bc25bcd59", "CreditProcessAdmin", "CREDITPROCESSADMIN" },
                    { "c6117596-6893-4163-b677-68cb0b6372c8", "12a3b636-3ad7-44f6-a9bd-2b0d6e19b5ed", "MosavabatAdmin", "MOSAVABATADMIN" },
                    { "f05f7f94-2986-4955-a91e-7c1482f31f7d", "3255faaf-9b7f-4da3-9d6b-381e8419ef39", "BizagiAdmin", "BIZAGIADMIN" }
                });
        }
    }
}
