using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPartsParser.Migrations
{
    public partial class add_locId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "MyPartsUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "MyPartsUsers");
        }
    }
}
