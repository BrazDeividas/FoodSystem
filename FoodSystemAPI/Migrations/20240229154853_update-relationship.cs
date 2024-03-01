using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class updaterelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingredient_recipe_RecipeId",
                table: "ingredient");

            migrationBuilder.DropTable(
                name: "recipe_ingredient");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_RecipeId",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "ingredient");

            migrationBuilder.CreateTable(
                name: "RecipeIngredient",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeIngredient", x => new { x.RecipeId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredient",
                        principalColumn: "ingredient_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeIngredient_recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "recipe",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_IngredientId",
                table: "RecipeIngredient",
                column: "IngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeIngredient");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "recipe_ingredient",
                columns: table => new
                {
                    ingredient_id = table.Column<int>(type: "int", nullable: false),
                    recipe_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    amount_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_recipe_ingredient_ingredient",
                        column: x => x.ingredient_id,
                        principalTable: "ingredient",
                        principalColumn: "ingredient_id");
                    table.ForeignKey(
                        name: "FK_recipe_ingredient_recipe",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "recipe_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_RecipeId",
                table: "ingredient",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_ingredient_ingredient_id",
                table: "recipe_ingredient",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_ingredient_recipe_id",
                table: "recipe_ingredient",
                column: "recipe_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ingredient_recipe_RecipeId",
                table: "ingredient",
                column: "RecipeId",
                principalTable: "recipe",
                principalColumn: "recipe_id");
        }
    }
}
