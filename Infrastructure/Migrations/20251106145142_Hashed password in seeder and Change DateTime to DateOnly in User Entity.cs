using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HashedpasswordinseederandChangeDateTimetoDateOnlyinUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Users",
                newName: "BirthDate");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthDate", "Password" },
                values: new object[] { new DateOnly(2000, 1, 1), "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BirthDate", "Password" },
                values: new object[] { new DateOnly(2000, 1, 1), "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BirthDate", "Password" },
                values: new object[] { new DateOnly(2000, 1, 1), "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Users",
                newName: "Date");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Password" },
                values: new object[] { new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Password" },
                values: new object[] { new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Password" },
                values: new object[] { new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234" });
        }
    }
}
