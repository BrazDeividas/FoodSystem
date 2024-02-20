using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    username = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    password_hash = table.Column<byte[]>(type: "binary(64)", fixedLength: true, maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "ingredient",
                columns: table => new
                {
                    ingredient_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    energy_kcal = table.Column<int>(type: "int", nullable: false),
                    protein_g = table.Column<double>(type: "float", nullable: false),
                    saturated_fats_g = table.Column<double>(type: "float", nullable: false),
                    fat_g = table.Column<double>(type: "float", nullable: false),
                    carb_g = table.Column<double>(type: "float", nullable: false),
                    fiber_g = table.Column<double>(type: "float", nullable: false),
                    sugar_g = table.Column<double>(type: "float", nullable: false),
                    calcium_mg = table.Column<double>(type: "float", nullable: false),
                    iron_mg = table.Column<double>(type: "float", nullable: false),
                    magnesium_mg = table.Column<double>(type: "float", nullable: false),
                    potassium_mg = table.Column<double>(type: "float", nullable: false),
                    sodium_mg = table.Column<double>(type: "float", nullable: false),
                    zinc_mg = table.Column<double>(type: "float", nullable: false),
                    copper_mcg = table.Column<double>(type: "float", nullable: false),
                    manganese_mg = table.Column<double>(type: "float", nullable: false),
                    selenium_mcg = table.Column<double>(type: "float", nullable: false),
                    vitc_mg = table.Column<double>(type: "float", nullable: false),
                    thiamin_mg = table.Column<double>(type: "float", nullable: false),
                    riboflavin_mg = table.Column<double>(type: "float", nullable: false),
                    niacin_mg = table.Column<double>(type: "float", nullable: false),
                    vitb6_mg = table.Column<double>(type: "float", nullable: false),
                    folate_mcg = table.Column<double>(type: "float", nullable: false),
                    vitb12_mcg = table.Column<double>(type: "float", nullable: false),
                    vita_mcg = table.Column<double>(type: "float", nullable: false),
                    vite_mg = table.Column<double>(type: "float", nullable: false),
                    vitd2_mcg = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient", x => x.ingredient_id);
                    table.ForeignKey(
                        name: "FK_ingredient_category",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "category_id");
                }); */

            migrationBuilder.CreateTable(
                name: "meal_plan",
                columns: table => new
                {
                    meal_plan_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_plan", x => x.meal_plan_id);
                    table.ForeignKey(
                        name: "FK_meal_plan_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            /* migrationBuilder.CreateTable(
                name: "recipe",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(4000)", unicode: false, maxLength: 4000, nullable: false),
                    preparation_time = table.Column<int>(type: "int", nullable: false),
                    cooking_time = table.Column<int>(type: "int", nullable: false),
                    servings = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe", x => x.recipe_id);
                    table.ForeignKey(
                        name: "FK_recipe_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                }); */

            migrationBuilder.CreateTable(
                name: "user_metrics",
                columns: table => new
                {
                    user_metrics_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    sex = table.Column<int>(type: "int", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    height = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<int>(type: "int", nullable: false),
                    activity_level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_metrics", x => x.user_metrics_id);
                    table.ForeignKey(
                        name: "FK_user_metrics_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "meal_plan_item",
                columns: table => new
                {
                    meal_plan_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meal_plan_id = table.Column<int>(type: "int", nullable: false),
                    recipe_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_plan_item", x => x.meal_plan_item_id);
                    table.ForeignKey(
                        name: "FK_meal_plan_item_meal_plan",
                        column: x => x.meal_plan_id,
                        principalTable: "meal_plan",
                        principalColumn: "meal_plan_id");
                    table.ForeignKey(
                        name: "FK_meal_plan_item_recipe",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "recipe_id");
                });

            /* migrationBuilder.CreateTable(
                name: "recipe_ingredient",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "int", nullable: false),
                    ingredient_id = table.Column<int>(type: "int", nullable: false),
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
                }); */

/*             migrationBuilder.CreateIndex(
                name: "IX_ingredient_category_id",
                table: "ingredient",
                column: "category_id"); */

            migrationBuilder.CreateIndex(
                name: "IX_meal_plan_user_id",
                table: "meal_plan",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_meal_plan_item_meal_plan_id",
                table: "meal_plan_item",
                column: "meal_plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_meal_plan_item_recipe_id",
                table: "meal_plan_item",
                column: "recipe_id");

/*             migrationBuilder.CreateIndex(
                name: "IX_recipe_user_id",
                table: "recipe",
                column: "user_id"); */

/*             migrationBuilder.CreateIndex(
                name: "IX_recipe_ingredient_ingredient_id",
                table: "recipe_ingredient",
                column: "ingredient_id"); */

/*             migrationBuilder.CreateIndex(
                name: "IX_recipe_ingredient_recipe_id",
                table: "recipe_ingredient",
                column: "recipe_id"); */

/*             migrationBuilder.CreateIndex(
                name: "IX_user",
                table: "user",
                column: "username",
                unique: true); */

            migrationBuilder.CreateIndex(
                name: "IX_user_metrics_user_id",
                table: "user_metrics",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "meal_plan_item");

            migrationBuilder.DropTable(
                name: "recipe_ingredient");

            migrationBuilder.DropTable(
                name: "user_metrics");

            migrationBuilder.DropTable(
                name: "meal_plan");

            migrationBuilder.DropTable(
                name: "ingredient");

            migrationBuilder.DropTable(
                name: "recipe");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
