using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipesGalorePRJ.Models;
using RecipesGalorePRJ.Pages.DBConnection;

namespace RecipesGalorePRJ.Pages.Admin
{
    public class AdminUpdateRecipesModel : PageModel
    {
        [BindProperty]
        public RecipeModel recipeUpdate { get; set; }
        public DatabaseConnection connection;

        [BindProperty(SupportsGet = true)]
        public IFormFile RecipeFileUpdate { get; set; }

        public readonly IWebHostEnvironment _env;
        public AdminUpdateRecipesModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult OnGet(int? id)
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            recipeUpdate = new RecipeModel();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM RecipeDBO WHERE RecipeID = @ID";
                command.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    recipeUpdate.RecipeID = reader.GetInt32(0);
                    recipeUpdate.Name = reader.GetString(1);
                    recipeUpdate.CuisineType = reader.GetString(2);
                    recipeUpdate.CookingTime = reader.GetString(3);
                    recipeUpdate.Ingredients = reader.GetString(4);
                    recipeUpdate.Method = reader.GetString(5);
                    recipeUpdate.File = reader.GetString(6);
                }
            }
            conn.Close();
            return Page();
        }

        public IActionResult OnPost()
        {
            var FileToUpload = Path.Combine(_env.WebRootPath, "Files", RecipeFileUpdate.FileName);

            using (var FStream = new FileStream(FileToUpload, FileMode.Create))
            {
                RecipeFileUpdate.CopyTo(FStream);
            }

            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"UPDATE RecipeDBO SET Name = @RecName, CuisineType = @RecCT, CookingTime = @RecCOOKT, Ingredients = @RecI, Method = @RecM, File = @RecF WHERE RecipeID = @RecID";

                command.Parameters.AddWithValue("@RecID", recipeUpdate.RecipeID);
                command.Parameters.AddWithValue("@RecName", recipeUpdate.Name);
                command.Parameters.AddWithValue("@RecCT", recipeUpdate.CuisineType);
                command.Parameters.AddWithValue("@RecCOOKT", recipeUpdate.CookingTime);
                command.Parameters.AddWithValue("@RecM", recipeUpdate.Ingredients);
                command.Parameters.AddWithValue("@RecI", recipeUpdate.Method);
                command.Parameters.AddWithValue("@RecF", RecipeFileUpdate.FileName);

                command.ExecuteNonQuery();
            }

            conn.Close();

            return RedirectToPage("/Admin/AdminViewAllRecipes");
        }
    }
}
