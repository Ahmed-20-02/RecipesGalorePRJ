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
    public class AdminDeleteRecipeModel : PageModel
    {
        [BindProperty]
        public RecipeModel recipeDelete { get; set; }
        public DatabaseConnection connection;

        public IActionResult OnGet(int? id)
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM RecipeDbo WHERE RecipeID = @ID";
                command.Parameters.AddWithValue("@ID", id);

                SqlDataReader reader = command.ExecuteReader();

                recipeDelete = new RecipeModel();

                while (reader.Read())
                {
                    recipeDelete.RecipeId = reader.GetInt32(0);
                    recipeDelete.RecipeName = reader.GetString(1);
                    recipeDelete.RecipeCuisineType = reader.GetString(2);
                    recipeDelete.RecipeCookingTime = reader.GetString(3);
                    recipeDelete.RecipeIngredients = reader.GetString(4);
                    recipeDelete.RecipeMethod = reader.GetString(5);
                    recipeDelete.File = reader.GetString(6);
                }

            }
            conn.Close();
            return Page();
        }

        public IActionResult OnPost()
        {
            string DbConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RecipesGaloreDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "DELETE RecipeDbo WHERE RecipeID = @ID";
                command.Parameters.AddWithValue("@ID", recipeDelete.RecipeId);
                command.ExecuteNonQuery();
            }
            conn.Close();
            return RedirectToPage("/Admin/ManageRecipes");
        }


    }
}
