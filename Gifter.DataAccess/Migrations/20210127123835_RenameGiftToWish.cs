using Microsoft.EntityFrameworkCore.Migrations;

namespace Gifter.DataAccess.Migrations
{
    public partial class RenameGiftToWish : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Gifts_GiftId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.RenameColumn(
                name: "GiftId",
                table: "Reservations",
                newName: "WishId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_GiftId",
                table: "Reservations",
                newName: "IX_Reservations_WishId");

            migrationBuilder.CreateTable(
                name: "Wishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    WishListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishes_Wishlists_WishListId",
                        column: x => x.WishListId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishes_WishListId",
                table: "Wishes",
                column: "WishListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Wishes_WishId",
                table: "Reservations",
                column: "WishId",
                principalTable: "Wishes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Wishes_WishId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Wishes");

            migrationBuilder.RenameColumn(
                name: "WishId",
                table: "Reservations",
                newName: "GiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_WishId",
                table: "Reservations",
                newName: "IX_Reservations_GiftId");

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WishListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gifts_Wishlists_WishListId",
                        column: x => x.WishListId,
                        principalTable: "Wishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_WishListId",
                table: "Gifts",
                column: "WishListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Gifts_GiftId",
                table: "Reservations",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id");
        }
    }
}
