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
    public class AdminAddRecipeModel : PageModel
    {
        [BindProperty]
        public RecipeModel recipe { get; set; }
        public DatabaseConnection connection;

        [BindProperty(SupportsGet = true)]
        public IFormFile RecipeFile { get; set; }

        public readonly IWebHostEnvironment _env;
        public AdminAddRecipeModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult OnPost()
        {
            var FileToUpload = Path.Combine(_env.WebRootPath, "Files", RecipeFile.FileName);
            Console.WriteLine("File Name : " + FileToUpload);

            using (var FStream = new FileStream(FileToUpload, FileMode.Create))
            {
                RecipeFile.CopyTo(FStream);
            }

            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO RecipeDbo (RecipeName, RecipeCuisineType, RecipeCookingTime, RecipeMethod, RecipeIngredients, RecipeFile) VALUES (@RN, @RCT, @RCOOKT, @RM, @RI, @RF)";

                command.Parameters.AddWithValue("@RN", recipe.RecipeName);
                command.Parameters.AddWithValue("@RCT", recipe.RecipeCuisineType);
                command.Parameters.AddWithValue("@RCOOKT", recipe.RecipeCookingTime);
                command.Parameters.AddWithValue("@RM", recipe.RecipeIngredients);
                command.Parameters.AddWithValue("@RI", recipe.RecipeMethod);
                command.Parameters.AddWithValue("@RF", RecipeFile.FileName);

                command.ExecuteNonQuery();
            }

            return RedirectToPage("/User/UserIndex");
        }
        public void OnGet()
        {
        }
    }
}
