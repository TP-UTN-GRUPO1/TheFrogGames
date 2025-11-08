using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Renamecolumsinuserentitityandsedeer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthDate", "IsDeleted", "Password" },
                values: new object[] { new DateOnly(2000, 1, 1), false, "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BirthDate", "IsDeleted", "Password" },
                values: new object[] { new DateOnly(2000, 1, 1), false, "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BirthDate", "IsDeleted", "Password" },
                values: new object[] { new DateOnly(2000, 1, 1), false, "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "IsActive", "Password" },
                values: new object[] { new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "1234" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "IsActive", "Password" },
                values: new object[] { new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "1234" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "IsActive", "Password" },
                values: new object[] { new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "1234" });
        }
    }
}
