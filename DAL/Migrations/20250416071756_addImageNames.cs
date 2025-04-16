using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addImageNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageBaseName",
                table: "Products",
                newName: "MainImageBaseName");

            migrationBuilder.AddColumn<string>(
                name: "ImageBaseNames",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBaseNames",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "MainImageBaseName",
                table: "Products",
                newName: "ImageBaseName");
        }
    }
}
