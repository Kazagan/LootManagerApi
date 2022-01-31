using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class removeMax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RollMax",
                table: "GoodTypeRoller");

            migrationBuilder.DropColumn(
                name: "RollMax",
                table: "GoodRoller");

            migrationBuilder.DropColumn(
                name: "RollMax",
                table: "CoinRoller");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RollMax",
                table: "GoodTypeRoller",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RollMax",
                table: "GoodRoller",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RollMax",
                table: "CoinRoller",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
