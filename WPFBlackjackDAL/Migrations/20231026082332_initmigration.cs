using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFBlackjackDAL.Migrations
{
    /// <inheritdoc />
    public partial class initmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hands",
                columns: table => new
                {
                    HandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsBlackJack = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hands", x => x.HandId);
                });

            migrationBuilder.CreateTable(
                name: "PlayerProfiles",
                columns: table => new
                {
                    PlayerName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Funds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProfiles", x => x.PlayerName);
                });

            migrationBuilder.CreateTable(
                name: "Shoes",
                columns: table => new
                {
                    ShoeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardsSinceLastShuffle = table.Column<int>(type: "int", nullable: false),
                    TotalCards = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoes", x => x.ShoeID);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suit = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    HandId = table.Column<int>(type: "int", nullable: true),
                    ShoeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardID);
                    table.ForeignKey(
                        name: "FK_Cards_Hands_HandId",
                        column: x => x.HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId");
                    table.ForeignKey(
                        name: "FK_Cards_Shoes_ShoeID",
                        column: x => x.ShoeID,
                        principalTable: "Shoes",
                        principalColumn: "ShoeID");
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pot = table.Column<int>(type: "int", nullable: false),
                    ShoeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_GameStates_Shoes_ShoeID",
                        column: x => x.ShoeID,
                        principalTable: "Shoes",
                        principalColumn: "ShoeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HandId = table.Column<int>(type: "int", nullable: false),
                    IsWinner = table.Column<bool>(type: "bit", nullable: false),
                    Funds = table.Column<int>(type: "int", nullable: false),
                    PlayerState = table.Column<int>(type: "int", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerNumber = table.Column<int>(type: "int", nullable: false),
                    IsDealer = table.Column<bool>(type: "bit", nullable: false),
                    GameStateGameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_GameStates_GameStateGameId",
                        column: x => x.GameStateGameId,
                        principalTable: "GameStates",
                        principalColumn: "GameId");
                    table.ForeignKey(
                        name: "FK_Players_Hands_HandId",
                        column: x => x.HandId,
                        principalTable: "Hands",
                        principalColumn: "HandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_HandId",
                table: "Cards",
                column: "HandId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ShoeID",
                table: "Cards",
                column: "ShoeID");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_ShoeID",
                table: "GameStates",
                column: "ShoeID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameStateGameId",
                table: "Players",
                column: "GameStateGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_HandId",
                table: "Players",
                column: "HandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "PlayerProfiles");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "Hands");

            migrationBuilder.DropTable(
                name: "Shoes");
        }
    }
}
