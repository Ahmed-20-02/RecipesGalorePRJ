using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipesGalorePRJ.Models;
using RecipesGalorePRJ.Pages.DBConnection;

namespace RecipesGalorePRJ.Pages.Admin
{
    public class AdminRecipeView : PageModel
    {

        public RecipeModel RecipeClass { get; set; }
        public DatabaseConnection connection;

        public IActionResult OnGet(int? ID)
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM RecipeDBO WHERE RecipeID = @ID";
                command.Parameters.AddWithValue("@ID", ID);

                SqlDataReader reader = command.ExecuteReader();
                RecipeClass = new RecipeModel();

                while (reader.Read())
                {
                    RecipeClass.RecipeID = reader.GetInt32(0);
                    RecipeClass.Name = reader.GetString(1);
                    RecipeClass.CuisineType = reader.GetString(2);
                    RecipeClass.CookingTime = reader.GetString(3);
                    RecipeClass.Ingredients = reader.GetString(4);
                    RecipeClass.Method = reader.GetString(5);
                    RecipeClass.File = reader.GetString(6);
                }
            }
            return Page();
        }
    }
}
