using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaToRing2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Rings_RingId",
                table: "Medias");

            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("ecda4eef-a458-43c9-8834-ce4851718667"));

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("655a26e4-5047-49cd-b70a-4f3491b0d59f"), null, "انگشتر عقیق", "R2732874204", "UID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Rings_RingId",
                table: "Medias",
                column: "RingId",
                principalTable: "Rings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Rings_RingId",
                table: "Medias");

            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("655a26e4-5047-49cd-b70a-4f3491b0d59f"));

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Serial", "Uid" },
                values: new object[] { new Guid("ecda4eef-a458-43c9-8834-ce4851718667"), null, "انگشتر عقیق", "R2732874204", "UID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Rings_RingId",
                table: "Medias",
                column: "RingId",
                principalTable: "Rings",
                principalColumn: "Id");
        }
    }
}
