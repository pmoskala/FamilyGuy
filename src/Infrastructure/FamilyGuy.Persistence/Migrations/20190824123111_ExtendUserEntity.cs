using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilyGuy.Persistence.Migrations
{
    public partial class ExtendUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelephoneNumber",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelephoneNumber",
                table: "Users");
        }
    }
}
