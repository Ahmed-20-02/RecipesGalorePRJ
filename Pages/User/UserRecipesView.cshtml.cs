using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipesGalorePRJ.Models;
using RecipesGalorePRJ.Pages.DBConnection;

namespace RecipesGalorePRJ.Pages.User
{
    public class UserRecipesViewModel : PageModel
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
                    RecipeClass.RecipeId = reader.GetInt32(0);
                    RecipeClass.RecipeName = reader.GetString(1);
                    RecipeClass.RecipeCuisineType = reader.GetString(2);
                    RecipeClass.RecipeCookingTime = reader.GetString(3);
                    RecipeClass.RecipeIngredients = reader.GetString(4);
                    RecipeClass.RecipeMethod = reader.GetString(5);
                    RecipeClass.File = reader.GetString(6);
                }
            }
            return Page();
        }
    }
}
