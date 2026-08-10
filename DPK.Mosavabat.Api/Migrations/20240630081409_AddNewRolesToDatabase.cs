using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class AddNewRolesToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4e30fa5-2c16-49f9-a7fb-97c50d7c9073");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6aa3943-a3eb-4fbb-9afc-d58b7d58993c");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "b4e30fa5-2c16-49f9-a7fb-97c50d7c9073", "5abfa524-1687-42ce-b595-622e92e373c7", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f6aa3943-a3eb-4fbb-9afc-d58b7d58993c", "5547deb1-995d-451b-83e9-0a74a3b38357", "Manager", "MANAGER" });
        }
    }
}
