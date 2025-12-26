using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaToRing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("588cf310-6cf3-4e22-abc8-001c0891aab5"));

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("ecda4eef-a458-43c9-8834-ce4851718667"), null, "انگشتر عقیق", "R2732874204", "UID" });

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
                keyValue: new Guid("ecda4eef-a458-43c9-8834-ce4851718667"));

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("588cf310-6cf3-4e22-abc8-001c0891aab5"), null, "انگشتر عقیق", "R2732874204", "UID" });
        }
    }
}
