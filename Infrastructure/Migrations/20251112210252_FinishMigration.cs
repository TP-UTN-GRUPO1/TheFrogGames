using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinishMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "BirthDate", "City", "Country", "Email", "IsDeleted", "LastName", "Name", "Password", "PokemonName", "Province", "RoleId" },
                values: new object[,]
                {
                    { 1, null, new DateOnly(2000, 1, 1), null, null, "sysadmin@demo.com", false, "SysAdmin", "SysAdmin", "5fca3522def0e33e6606d9ec2ccbf8d38a1f02321e3b084c7099b6bcdbac2f53", null, null, 1 },
                    { 2, null, new DateOnly(2000, 1, 1), null, null, "admin@demo.com", false, "Admin", "Admin", "5fca3522def0e33e6606d9ec2ccbf8d38a1f02321e3b084c7099b6bcdbac2f53", null, null, 2 },
                    { 3, null, new DateOnly(2000, 1, 1), null, null, "user@demo.com", false, "User", "User", "5fca3522def0e33e6606d9ec2ccbf8d38a1f02321e3b084c7099b6bcdbac2f53", null, null, 3 }
                });
        }
    }
}
