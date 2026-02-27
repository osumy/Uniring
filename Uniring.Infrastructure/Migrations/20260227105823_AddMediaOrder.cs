using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("655a26e4-5047-49cd-b70a-4f3491b0d59f"));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Medias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rings_Serial",
                table: "Rings",
                column: "Serial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rings_Uid",
                table: "Rings",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rings_Serial",
                table: "Rings");

            migrationBuilder.DropIndex(
                name: "IX_Rings_Uid",
                table: "Rings");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Medias");

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("655a26e4-5047-49cd-b70a-4f3491b0d59f"), null, "انگشتر عقیق", "R2732874204", "UID" });
        }
    }
}
