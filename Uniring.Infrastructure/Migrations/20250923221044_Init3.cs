using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uniring.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("263d765b-c441-4a7b-94f3-f510b6d04fbc"));

            migrationBuilder.RenameColumn(
                name: "uid",
                table: "Rings",
                newName: "Uid");

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Price", "Serial", "Uid" },
                values: new object[] { new Guid("a11136c4-dc19-47f5-b133-501857e126c4"), null, "انگشتر عقیق", 25, "R2732874204", "UID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rings",
                keyColumn: "Id",
                keyValue: new Guid("a11136c4-dc19-47f5-b133-501857e126c4"));

            migrationBuilder.RenameColumn(
                name: "Uid",
                table: "Rings",
                newName: "uid");

            migrationBuilder.InsertData(
                table: "Rings",
                columns: new[] { "Id", "Description", "Name", "Price", "Serial", "uid" },
                values: new object[] { new Guid("263d765b-c441-4a7b-94f3-f510b6d04fbc"), null, "انگشتر عقیق", 25, "R2732874204", "UID" });
        }
    }
}
