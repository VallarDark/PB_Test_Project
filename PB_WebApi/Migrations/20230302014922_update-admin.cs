using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PB_WebApi.Migrations
{
    public partial class updateadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "6f2675a5-b318-4c2d-ad1b-f506c6d617ec");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3215941a-5cac-4923-97cc-6b0a936cd3ad");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "18290e13-d6c9-4670-9fb7-a3d1813a0435");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "18290e13-d6c9-4670-9fb7-a3d1813a0435", 1 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "6f2675a5-b318-4c2d-ad1b-f506c6d617ec", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastName", "Name", "NickName", "Password", "RoleId", "SessionToken" },
                values: new object[] { "3215941a-5cac-4923-97cc-6b0a936cd3ad", "admin@email.com", "Admin", "Admin", "Admin", "UXdlcnR5MTIzIQ==", "18290e13-d6c9-4670-9fb7-a3d1813a0435", "18290e13-d6c9-4670-9fb7-a3d1813a0435" });
        }
    }
}
