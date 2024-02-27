using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class updaterecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_meal_plan_item_recipe_id",
                table: "meal_plan_item");

            migrationBuilder.DropColumn(
                name: "cooking_time",
                table: "recipe");

            migrationBuilder.RenameColumn(
                name: "preparation_time",
                table: "recipe",
                newName: "calories");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "recipe",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "recipe",
                newName: "instructions");

            migrationBuilder.RenameColumn(
                name: "TotalCalories",
                table: "meal_plan",
                newName: "total_calories");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "recipe",
                type: "varchar(256)",
                unicode: false,
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_meal_plan_item_recipe_id",
                table: "meal_plan_item",
                column: "recipe_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_RecipeId",
                table: "ingredient",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ingredient_recipe_RecipeId",
                table: "ingredient",
                column: "RecipeId",
                principalTable: "recipe",
                principalColumn: "recipe_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingredient_recipe_RecipeId",
                table: "ingredient");

            migrationBuilder.DropIndex(
                name: "IX_meal_plan_item_recipe_id",
                table: "meal_plan_item");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_RecipeId",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "recipe");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "ingredient");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "recipe",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "instructions",
                table: "recipe",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "calories",
                table: "recipe",
                newName: "preparation_time");

            migrationBuilder.RenameColumn(
                name: "total_calories",
                table: "meal_plan",
                newName: "TotalCalories");

            migrationBuilder.AddColumn<int>(
                name: "cooking_time",
                table: "recipe",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_meal_plan_item_recipe_id",
                table: "meal_plan_item",
                column: "recipe_id");
        }
    }
}
