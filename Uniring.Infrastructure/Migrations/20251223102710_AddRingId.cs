using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("841945d0-a170-4915-b388-941554dda619"));

            migrationBuilder.AddColumn<Guid>(
                name: "RingId",
                table: "Medias",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("5a56f52e-9072-4334-b47c-afe089fcfdb4"), null, "انگشتر عقیق", "R2732874204", "UID" });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_RingId",
                table: "Medias",
                column: "RingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Rings_RingId",
                table: "Medias",
                column: "RingId",
                principalTable: "Rings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Rings_RingId",
                table: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Medias_RingId",
                table: "Medias");

            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("5a56f52e-9072-4334-b47c-afe089fcfdb4"));

            migrationBuilder.DropColumn(
                name: "RingId",
                table: "Medias");

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("841945d0-a170-4915-b388-941554dda619"), null, "انگشتر عقیق", "R2732874204", "UID" });
        }
    }
}
