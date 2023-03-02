using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PB_WebApi.Migrations
{
    public partial class addconversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "db295a73-5163-4ced-912b-1af73909a009");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "34a44cbe-cc23-4e88-8355-5f694ab53492");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a808ffe3-6b95-4a1f-9093-3f9873078b3e");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "de5ae337-d4e9-47e2-9195-9823fff52693", 0 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "e99a720a-dbf3-4b92-b264-61db0d8feddb", 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastName", "Name", "NickName", "Password", "RoleId", "SessionToken" },
                values: new object[] { "09886728-fc04-4aac-8ace-fef1b3615bb9", "admin@email.com", "Admin", "Admin", "Administrator", "UXdlcnR5MTIzIQ==", "e99a720a-dbf3-4b92-b264-61db0d8feddb", "e99a720a-dbf3-4b92-b264-61db0d8feddb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "de5ae337-d4e9-47e2-9195-9823fff52693");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "09886728-fc04-4aac-8ace-fef1b3615bb9");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e99a720a-dbf3-4b92-b264-61db0d8feddb");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "a808ffe3-6b95-4a1f-9093-3f9873078b3e", 1 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "db295a73-5163-4ced-912b-1af73909a009", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastName", "Name", "NickName", "Password", "RoleId", "SessionToken" },
                values: new object[] { "34a44cbe-cc23-4e88-8355-5f694ab53492", "admin@email.com", "Admin", "Admin", "Administrator", "UXdlcnR5MTIzIQ==", "a808ffe3-6b95-4a1f-9093-3f9873078b3e", "a808ffe3-6b95-4a1f-9093-3f9873078b3e" });
        }
    }
}
