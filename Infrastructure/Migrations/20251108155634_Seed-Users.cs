using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "SysAdmin" },
                    { 2, "Admin" },
                    { 3, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "BirthDate", "City", "Country", "Email", "IsDeleted", "LastName", "Name", "Password", "Province", "RoleId" },
                values: new object[,]
                {
                    { 1, null, new DateOnly(2000, 1, 1), null, null, "sysadmin@demo.com", false, "SysAdmin", "SysAdmin", "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4", null, 1 },
                    { 2, null, new DateOnly(2000, 1, 1), null, null, "admin@demo.com", false, "Admin", "Admin", "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4", null, 2 },
                    { 3, null, new DateOnly(2000, 1, 1), null, null, "user@demo.com", false, "User", "User", "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4", null, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
