using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternalAPI.Migrations
{
    /// <inheritdoc />
    public partial class updaterecipemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IngredientIds",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientIds",
                table: "Recipes");
        }
    }
}
