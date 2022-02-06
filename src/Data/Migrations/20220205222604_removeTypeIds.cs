using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class removeTypeIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinType_Coin_Id",
                table: "CoinType");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodType_Good_Id",
                table: "GoodType");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodType_GoodTypeRoller_Id",
                table: "GoodType");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GoodType",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CoinType",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_GoodTypeRoller_GoodTypeId",
                table: "GoodTypeRoller",
                column: "GoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Good_GoodTypeId",
                table: "Good",
                column: "GoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Coin_CoinTypeId",
                table: "Coin",
                column: "CoinTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coin_CoinType_CoinTypeId",
                table: "Coin",
                column: "CoinTypeId",
                principalTable: "CoinType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Good_GoodType_GoodTypeId",
                table: "Good",
                column: "GoodTypeId",
                principalTable: "GoodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoodTypeRoller_GoodType_GoodTypeId",
                table: "GoodTypeRoller",
                column: "GoodTypeId",
                principalTable: "GoodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coin_CoinType_CoinTypeId",
                table: "Coin");

            migrationBuilder.DropForeignKey(
                name: "FK_Good_GoodType_GoodTypeId",
                table: "Good");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodTypeRoller_GoodType_GoodTypeId",
                table: "GoodTypeRoller");

            migrationBuilder.DropIndex(
                name: "IX_GoodTypeRoller_GoodTypeId",
                table: "GoodTypeRoller");

            migrationBuilder.DropIndex(
                name: "IX_Good_GoodTypeId",
                table: "Good");

            migrationBuilder.DropIndex(
                name: "IX_Coin_CoinTypeId",
                table: "Coin");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GoodType",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CoinType",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinType_Coin_Id",
                table: "CoinType",
                column: "Id",
                principalTable: "Coin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoodType_Good_Id",
                table: "GoodType",
                column: "Id",
                principalTable: "Good",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoodType_GoodTypeRoller_Id",
                table: "GoodType",
                column: "Id",
                principalTable: "GoodTypeRoller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
