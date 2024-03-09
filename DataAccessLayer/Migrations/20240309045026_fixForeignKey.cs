using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class fixForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_ArtworkTypes_ArtworkTypeId",
                table: "Artworks");

            migrationBuilder.DropIndex(
                name: "IX_Artworks_ArtworkTypeId",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "ArtworkTypeId",
                table: "Artworks");

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_TypeId",
                table: "Artworks",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_ArtworkTypes_TypeId",
                table: "Artworks",
                column: "TypeId",
                principalTable: "ArtworkTypes",
                principalColumn: "artworkType_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_ArtworkTypes_TypeId",
                table: "Artworks");

            migrationBuilder.DropIndex(
                name: "IX_Artworks_TypeId",
                table: "Artworks");

            migrationBuilder.AddColumn<int>(
                name: "ArtworkTypeId",
                table: "Artworks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_ArtworkTypeId",
                table: "Artworks",
                column: "ArtworkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_ArtworkTypes_ArtworkTypeId",
                table: "Artworks",
                column: "ArtworkTypeId",
                principalTable: "ArtworkTypes",
                principalColumn: "artworkType_Id");
        }
    }
}
