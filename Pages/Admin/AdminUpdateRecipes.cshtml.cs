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
                command.CommandText = @"SELECT * FROM RecipeDbo WHERE RecipeID = @ID";
                command.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    recipeUpdate.RecipeId = reader.GetInt32(0);
                    recipeUpdate.RecipeName = reader.GetString(1);
                    recipeUpdate.RecipeCuisineType = reader.GetString(2);
                    recipeUpdate.RecipeCookingTime = reader.GetString(3);
                    recipeUpdate.RecipeIngredients = reader.GetString(4);
                    recipeUpdate.RecipeMethod = reader.GetString(5);
                    recipeUpdate.File = reader.GetString(6);
                }
            }
            conn.Close();
            return Page();
        }

        public IActionResult OnPost()
        {
            var FileToUpload = Path.Combine(_env.WebRootPath, "Files", RecipeFileUpdate.FileName);
            Console.WriteLine("File Name : " + FileToUpload);

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
                command.CommandText = @"UPDATE RecipeDbo SET RecipeName = @RecName, RecipeCuisineType = @RecCT, RecipeCookingTime = @RecCOOKT, RecipeKeyword = @RecK, RecipeMethod = @RecM, RecipeIngredients = @RecI, RecipeFile = @RecF WHERE RecipeID = @RecID";

                command.Parameters.AddWithValue("@RecID", recipeUpdate.RecipeId);
                command.Parameters.AddWithValue("@RecName", recipeUpdate.RecipeName);
                command.Parameters.AddWithValue("@RecCT", recipeUpdate.RecipeCuisineType);
                command.Parameters.AddWithValue("@RecCOOKT", recipeUpdate.RecipeCookingTime);
                command.Parameters.AddWithValue("@RecM", recipeUpdate.RecipeIngredients);
                command.Parameters.AddWithValue("@RecI", recipeUpdate.RecipeMethod);
                command.Parameters.AddWithValue("@RecF", RecipeFileUpdate.FileName);

                command.ExecuteNonQuery();
            }

            conn.Close();

            return RedirectToPage("/Admin/AdminIndex");
        }
    }
}
