using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecipeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SourceAPI",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceAPI",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Recipes");
        }
    }
}
