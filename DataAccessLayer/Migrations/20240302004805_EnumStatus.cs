using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class EnumStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkStatuses");

            migrationBuilder.AddColumn<int>(
                name: "ArtworkStatus",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtworkStatus",
                table: "Artworks");

            migrationBuilder.CreateTable(
                name: "ArtworkStatuses",
                columns: table => new
                {
                    artworkStatus_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkStatuses", x => x.artworkStatus_Id);
                });
        }
    }
}
