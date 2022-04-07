using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InGold = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false),
                    Name = table.Column<string>(type: "Varchar", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "Varchar", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoinRoller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TreasureLevel = table.Column<int>(type: "integer", nullable: false),
                    RollMin = table.Column<int>(type: "integer", nullable: false),
                    CoinId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiceCount = table.Column<int>(type: "integer", nullable: false),
                    DiceSides = table.Column<int>(type: "integer", nullable: false),
                    Multiplier = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinRoller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinRoller_Coin_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Good",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "Varchar", maxLength: 250, nullable: false),
                    CoinId = table.Column<Guid>(type: "uuid", nullable: false),
                    GoodTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Good", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Good_Coin_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Good_GoodType_GoodTypeId",
                        column: x => x.GoodTypeId,
                        principalTable: "GoodType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodTypeRoller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TreasureLevel = table.Column<int>(type: "integer", nullable: false),
                    RollMin = table.Column<int>(type: "integer", nullable: false),
                    GoodTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodTypeRoller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodTypeRoller_GoodType_GoodTypeId",
                        column: x => x.GoodTypeId,
                        principalTable: "GoodType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodRoller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RollMin = table.Column<int>(type: "integer", nullable: false),
                    DiceCount = table.Column<int>(type: "integer", nullable: false),
                    DiceSides = table.Column<int>(type: "integer", nullable: false),
                    Multiplier = table.Column<int>(type: "integer", nullable: false),
                    GoodId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "IX_CoinRoller_TreasureLevel_RollMin",
                table: "CoinRoller",
                columns: new[] { "TreasureLevel", "RollMin" });

            migrationBuilder.CreateIndex(
                name: "IX_Good_CoinId",
                table: "Good",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_Good_GoodTypeId",
                table: "Good",
                column: "GoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodRoller_GoodId",
                table: "GoodRoller",
                column: "GoodId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodTypeRoller_GoodTypeId",
                table: "GoodTypeRoller",
                column: "GoodTypeId");
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

            migrationBuilder.DropTable(
                name: "Coin");

            migrationBuilder.DropTable(
                name: "GoodType");
        }
    }
}
