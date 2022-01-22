using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Goods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinTable");

            migrationBuilder.CreateTable(
                name: "CoinRoller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasureLevel = table.Column<int>(type: "int", nullable: false),
                    RollMin = table.Column<int>(type: "int", nullable: false),
                    RollMax = table.Column<int>(type: "int", nullable: false),
                    CoinId = table.Column<int>(type: "int", nullable: true),
                    DiceCount = table.Column<int>(type: "int", nullable: false),
                    DiceSides = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinRoller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinRoller_Coin_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coin",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Good",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Good", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Good_Coin_ValueId",
                        column: x => x.ValueId,
                        principalTable: "Coin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodTypeRoller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TreasureLevel = table.Column<int>(type: "int", nullable: false),
                    RollMin = table.Column<int>(type: "int", nullable: false),
                    RollMax = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodTypeRoller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodRoller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RollMin = table.Column<int>(type: "int", nullable: false),
                    RollMax = table.Column<int>(type: "int", nullable: false),
                    DiceCount = table.Column<int>(type: "int", nullable: false),
                    DiceSides = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<int>(type: "int", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodRoller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodRoller_Good_GoodId",
                        column: x => x.GoodId,
                        principalTable: "Good",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinRoller_CoinId",
                table: "CoinRoller",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_Good_ValueId",
                table: "Good",
                column: "ValueId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodRoller_GoodId",
                table: "GoodRoller",
                column: "GoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinRoller");

            migrationBuilder.DropTable(
                name: "GoodRoller");

            migrationBuilder.DropTable(
                name: "GoodTypeRoller");

            migrationBuilder.DropTable(
                name: "Good");

            migrationBuilder.CreateTable(
                name: "CoinTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoinId = table.Column<int>(type: "int", nullable: false),
                    DiceCount = table.Column<int>(type: "int", nullable: false),
                    DiceSides = table.Column<int>(type: "int", nullable: false),
                    Max = table.Column<int>(type: "int", nullable: false),
                    Min = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<int>(type: "int", nullable: false),
                    TreasureLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinTable_Coin_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinTable_CoinId",
                table: "CoinTable",
                column: "CoinId");
        }
    }
}
