using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("bf706b70-1035-4a6f-a28d-2d20849b7ba7"));

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("841945d0-a170-4915-b388-941554dda619"), null, "انگشتر عقیق", "R2732874204", "UID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("841945d0-a170-4915-b388-941554dda619"));

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("bf706b70-1035-4a6f-a28d-2d20849b7ba7"), null, "انگشتر عقیق", "R2732874204", "UID" });
        }
    }
}
