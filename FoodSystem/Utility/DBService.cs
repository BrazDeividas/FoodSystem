using System.Data.SqlClient;
using System.Data;
using Model;

namespace Utility
{
    public class DBService
    {
        public string connectionString { get; set; }

        public DBService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int CreateCommand(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                return command.ExecuteNonQuery();
            }
        }

        public int InsertIngredient(Ingredient ingredient)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO ingredient(" +
                "category_id, description, energy_kcal, protein_g, saturated_fats_g, fat_g, carb_g, fiber_g, sugar_g, calcium_mg, iron_mg, magnesium_mg, potassium_mg, " +
                "sodium_mg, zinc_mg, copper_mcg, manganese_mg, selenium_mcg, vitc_mg, thiamin_mg, riboflavin_mg, niacin_mg, vitb6_mg, folate_mcg, vitb12_mcg, vita_mcg, " +
                "vite_mg, vitd2_mcg)" +
                "VALUES(@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11, @param12, @param13, @param14, @param15, @param16, " +
                "@param17, @param18, @param19, @param20, @param21, @param22, @param23, @param24, @param25, @param26, @param27, @param28)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@param1", SqlDbType.Int).Value = ingredient.CategoryId;
                    command.Parameters.Add("@param2", SqlDbType.VarChar).Value = ingredient.Description;
                    command.Parameters.Add("@param3", SqlDbType.Int).Value = ingredient.Energy_kcal;
                    command.Parameters.Add("@param4", SqlDbType.Float).Value = ingredient.Protein_g;
                    command.Parameters.Add("@param5", SqlDbType.Float).Value = ingredient.Saturated_fats_g;
                    command.Parameters.Add("@param6", SqlDbType.Float).Value = ingredient.Fat_g;
                    command.Parameters.Add("@param7", SqlDbType.Float).Value = ingredient.Carb_g;
                    command.Parameters.Add("@param8", SqlDbType.Float).Value = ingredient.Fiber_g;
                    command.Parameters.Add("@param9", SqlDbType.Float).Value = ingredient.Sugar_g;
                    command.Parameters.Add("@param10", SqlDbType.Float).Value = ingredient.Calcium_mg;
                    command.Parameters.Add("@param11", SqlDbType.Float).Value = ingredient.Iron_mg;
                    command.Parameters.Add("@param12", SqlDbType.Float).Value = ingredient.Magnesium_mg;
                    command.Parameters.Add("@param13", SqlDbType.Float).Value = ingredient.Potassium_mg;
                    command.Parameters.Add("@param14", SqlDbType.Float).Value = ingredient.Sodium_mg;
                    command.Parameters.Add("@param15", SqlDbType.Float).Value = ingredient.Zinc_mg;
                    command.Parameters.Add("@param16", SqlDbType.Float).Value = ingredient.Copper_mcg;
                    command.Parameters.Add("@param17", SqlDbType.Float).Value = ingredient.Manganese_mg;
                    command.Parameters.Add("@param18", SqlDbType.Float).Value = ingredient.Selenium_mcg;
                    command.Parameters.Add("@param19", SqlDbType.Float).Value = ingredient.Vitc_mg;
                    command.Parameters.Add("@param20", SqlDbType.Float).Value = ingredient.Thiamin_mg;
                    command.Parameters.Add("@param21", SqlDbType.Float).Value = ingredient.Riboflavin_mg;
                    command.Parameters.Add("@param22", SqlDbType.Float).Value = ingredient.Niacin_mg;
                    command.Parameters.Add("@param23", SqlDbType.Float).Value = ingredient.Vitb6_mg;
                    command.Parameters.Add("@param24", SqlDbType.Float).Value = ingredient.Folate_mcg;
                    command.Parameters.Add("@param25", SqlDbType.Float).Value = ingredient.Vitb12_mcg;
                    command.Parameters.Add("@param26", SqlDbType.Float).Value = ingredient.Vita_mcg;
                    command.Parameters.Add("@param27", SqlDbType.Float).Value = ingredient.Vite_mg;
                    command.Parameters.Add("@param28", SqlDbType.Float).Value = ingredient.Vitd2_mcg;
                    command.CommandType = CommandType.Text;
                    return command.ExecuteNonQuery();
                }
            }

        public List<Category> ReadCategories() 
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM dbo.category";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(
                        new Category
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1)
                        }
                    );
                }
            }

            return categories;
        }

        public void TestConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select 1", connection);
                command.ExecuteNonQuery();
            }
        }
    }
}