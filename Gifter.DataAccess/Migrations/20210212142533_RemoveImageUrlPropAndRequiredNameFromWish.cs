using Microsoft.EntityFrameworkCore.Migrations;

namespace Gifter.DataAccess.Migrations
{
    public partial class RemoveImageUrlPropAndRequiredNameFromWish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Wishes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Wishes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Wishes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Wishes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
