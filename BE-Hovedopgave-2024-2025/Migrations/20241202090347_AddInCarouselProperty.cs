using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE_Hovedopgave_2024_2025.Migrations
{
    /// <inheritdoc />
    public partial class AddInCarouselProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InCarousel",
                table: "Products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InCarousel",
                table: "Products");
        }
    }
}
