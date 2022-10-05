using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherOfCity.Migrations
{
    public partial class Addtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "time_Id",
                table: "Weather",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_Id",
                table: "Weather");
        }
    }
}
