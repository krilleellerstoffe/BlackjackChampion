using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPFBlackjackDAL.Migrations
{
    /// <inheritdoc />
    public partial class statesmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "GameStates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "GameStates");
        }
    }
}
