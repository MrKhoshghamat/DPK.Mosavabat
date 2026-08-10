using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPK.Mosavabat.Api.Migrations
{
    public partial class ReverseChangeSomePaymentPermissionFieldsToNullabe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b4e30fa5-2c16-49f9-a7fb-97c50d7c9073", "5abfa524-1687-42ce-b595-622e92e373c7", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f6aa3943-a3eb-4fbb-9afc-d58b7d58993c", "5547deb1-995d-451b-83e9-0a74a3b38357", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4e30fa5-2c16-49f9-a7fb-97c50d7c9073");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6aa3943-a3eb-4fbb-9afc-d58b7d58993c");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrepaidAmount",
                table: "PaymentPermissions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "101d4bd2-4342-4884-9a80-bc3468e938fb", "b0670cd5-c77e-46d2-a1dd-5e54ba3257e6", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2f176c76-dcb7-4b25-9726-37fb20902cd9", "8ea0bdd3-ea53-4985-9c17-53b61d058064", "Manager", "MANAGER" });
        }
    }
}
