using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddSettingsFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomePageNewsPreContent",
                table: "Settings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHomePageGalleryPostTextVisible",
                table: "Settings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHomePageGalleryPreTextVisible",
                table: "Settings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomePageNewsPreContent",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsHomePageGalleryPostTextVisible",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IsHomePageGalleryPreTextVisible",
                table: "Settings");
        }
    }
}
