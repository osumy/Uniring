using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("7fb9e9d6-c033-4708-b2a2-37cc420c9bff"));

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Price", "Serial", "uid" },
                values: new object[] { new Guid("263d765b-c441-4a7b-94f3-f510b6d04fbc"), null, "انگشتر عقیق", 25, "R2732874204", "UID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("263d765b-c441-4a7b-94f3-f510b6d04fbc"));

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LastPurchaseAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Price", "Serial", "uid" },
                values: new object[] { new Guid("7fb9e9d6-c033-4708-b2a2-37cc420c9bff"), null, "انگشتر عقیق", 25, "R2732874204", "UID" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_PhoneNumber",
                table: "ApplicationUser",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }
    }
}
