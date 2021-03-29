using Microsoft.EntityFrameworkCore.Migrations;

namespace Gifter.DataAccess.Migrations
{
    public partial class AddFileNameAndDirNameForWishImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DirectoryName",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectoryName",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Images");
        }
    }
}
