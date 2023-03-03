using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PB_WebApi.Migrations
{
    public partial class updateadminpassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "22df5a2c-6e6d-44c6-b651-5d6f46d09747");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5e330268-f8e3-463c-9487-696ddff86a67");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "138e2bd1-335d-41e8-b327-e29f10650c38");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "19e23249-8754-4cd5-850f-947dee37d402", 1 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "b6fd2c21-dbe2-41c2-9090-9085f7c126f2", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastName", "Name", "NickName", "Password", "RepositoryType", "RoleId", "SessionToken" },
                values: new object[] { "ea516043-3498-4966-b4ac-fe0c6fd07455", "admin@email.com", "Admin", "Admin", "Administrator", "UXdlcnR5MTIzIQ==", 0, "19e23249-8754-4cd5-850f-947dee37d402", "19e23249-8754-4cd5-850f-947dee37d402" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b6fd2c21-dbe2-41c2-9090-9085f7c126f2");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "ea516043-3498-4966-b4ac-fe0c6fd07455");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "19e23249-8754-4cd5-850f-947dee37d402");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "138e2bd1-335d-41e8-b327-e29f10650c38", 1 });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleType" },
                values: new object[] { "22df5a2c-6e6d-44c6-b651-5d6f46d09747", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastName", "Name", "NickName", "Password", "RepositoryType", "RoleId", "SessionToken" },
                values: new object[] { "5e330268-f8e3-463c-9487-696ddff86a67", "admin@email.com", "Admin", "Admin", "Administrator", "Qwerty123!", 0, "138e2bd1-335d-41e8-b327-e29f10650c38", "138e2bd1-335d-41e8-b327-e29f10650c38" });
        }
    }
}
