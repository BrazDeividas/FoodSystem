using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatemealplan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalCalories",
                table: "meal_plan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCalories",
                table: "meal_plan");
        }
    }
}
