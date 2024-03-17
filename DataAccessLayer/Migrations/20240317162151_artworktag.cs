using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class artworktag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkTag");

            migrationBuilder.CreateTable(
                name: "ArtworkTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtworkId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtworkTags_Artworks_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "Artworks",
                        principalColumn: "artwork_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtworkTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "tag_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkTags_ArtworkId",
                table: "ArtworkTags",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkTags_TagId",
                table: "ArtworkTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkTags");

            migrationBuilder.CreateTable(
                name: "ArtworkTag",
                columns: table => new
                {
                    ArtworksId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkTag", x => new { x.ArtworksId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ArtworkTag_Artworks_ArtworksId",
                        column: x => x.ArtworksId,
                        principalTable: "Artworks",
                        principalColumn: "artwork_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtworkTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "tag_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkTag_TagsId",
                table: "ArtworkTag",
                column: "TagsId");
        }
    }
}
