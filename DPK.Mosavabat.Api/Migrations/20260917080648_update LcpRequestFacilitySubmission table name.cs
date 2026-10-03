using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class updateLcpRequestFacilitySubmissiontablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LCP_RequestFacilitySubmission",
                table: "LCP_RequestFacilitySubmission");

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

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "LCP_RequestFacilitySubmission",
                newName: "LcpRequestFacilitySubmission",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LcpRequestFacilitySubmission",
                schema: "dbo",
                table: "LcpRequestFacilitySubmission",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "225ad253-6534-4b8d-9263-ff0a011ae715", "4d452624-be9e-462a-a1d1-ef8de14878a9", "DpkAdmin", "DPKADMIN" },
                    { "797924e3-a0b1-4922-9f81-36106b72e821", "5a05cac9-3f13-4ea8-adfb-fd4ca06389ba", "MosavabatAdmin", "MOSAVABATADMIN" },
                    { "85742c2e-2e89-442f-a6e5-2fb1ef5b5fa6", "35fbd554-91d3-48cd-b69c-0ee41c2b9de8", "LoanManagementSystemAdmin", "LOANMANAGEMENTSYSTEMADMIN" },
                    { "aadc51cf-bb76-4ab8-a1be-388ae74715f2", "fd44cb2d-831d-4717-8541-09b1d2f08ed2", "BizagiAdmin", "BIZAGIADMIN" },
                    { "d3f12ebf-0c12-49f3-aae3-577c6fa73db9", "2644b96e-9b2e-4873-9277-3724c10b9243", "LegalCustomerPortalAdmin", "LEGALCUSTOMERPORTALADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LcpRequestFacilitySubmission",
                schema: "dbo",
                table: "LcpRequestFacilitySubmission");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "225ad253-6534-4b8d-9263-ff0a011ae715");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "797924e3-a0b1-4922-9f81-36106b72e821");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85742c2e-2e89-442f-a6e5-2fb1ef5b5fa6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aadc51cf-bb76-4ab8-a1be-388ae74715f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d3f12ebf-0c12-49f3-aae3-577c6fa73db9");

            migrationBuilder.RenameTable(
                name: "LcpRequestFacilitySubmission",
                schema: "dbo",
                newName: "LCP_RequestFacilitySubmission");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LCP_RequestFacilitySubmission",
                table: "LCP_RequestFacilitySubmission",
                column: "Id");

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
        }
    }
}
