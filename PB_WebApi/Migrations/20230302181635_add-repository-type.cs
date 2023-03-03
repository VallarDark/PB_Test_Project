using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PB_WebApi.Migrations
{
    public partial class addrepositorytype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RepositoryType",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "RepositoryType",
                table: "Users");

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
    }
}
