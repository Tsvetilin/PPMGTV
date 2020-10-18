using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddAboutColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "ContactLetters",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
