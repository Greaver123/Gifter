using Microsoft.EntityFrameworkCore.Migrations;

namespace Gifter.DataAccess.Migrations
{
    public partial class RemoveGiftType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_GiftType_GiftTypeId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_GiftTypeId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "GiftTypeId",
                table: "Gifts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GiftTypeId",
                table: "Gifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_GiftTypeId",
                table: "Gifts",
                column: "GiftTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_GiftType_GiftTypeId",
                table: "Gifts",
                column: "GiftTypeId",
                principalTable: "GiftType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
