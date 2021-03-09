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
    public class AdminManageRecipesModel : PageModel
    {
        public List<RecipeModel> recipesList { get; set; }
        public DatabaseConnection connection;

        public IActionResult OnGet()
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM RecipeDBO";

                SqlDataReader reader = command.ExecuteReader();

                recipesList = new List<RecipeModel>();

                while (reader.Read())
                {
                    RecipeModel rec = new RecipeModel();
                    rec.RecipeID = reader.GetInt32(0);
                    rec.Name = reader.GetString(1);
                    rec.CuisineType = reader.GetString(2);
                    rec.CookingTime = reader.GetString(3);
                    rec.Ingredients = reader.GetString(4);
                    rec.Method = reader.GetString(5);
                    rec.File = reader.GetString(6);

                    recipesList.Add(rec);
                }

                reader.Close();
            }
            return Page();
        }
    }
}
