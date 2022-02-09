using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class index_ForUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CoinRoller_TreasureLevel_RollMin",
                table: "CoinRoller",
                columns: new[] { "TreasureLevel", "RollMin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoinRoller_TreasureLevel_RollMin",
                table: "CoinRoller");
        }
    }
}
