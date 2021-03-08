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
    public class UserSearchRecipePageModel : PageModel
    {
        public List<RecipeModel> RecipeList = new List<RecipeModel>();
        [BindProperty]
        public string SearchString { get; set; }
        public DatabaseConnection connection;

        public void OnPost()
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;

                if (string.IsNullOrEmpty(SearchString))
                {
                    command.CommandText = @"SELECT * FROM Recipes WHERE RecipeName = t";
                }
                else
                {
                    command.CommandText = @"SELECT * FROM Recipes WHERE (RecipeName LIKE '%' + @SearchS) OR (RecipeName LIKE @SearchS + '%')";
                    command.Parameters.AddWithValue("@SearchS", SearchString);
                }

                SqlDataReader reader = command.ExecuteReader();

                RecipeList = new List<RecipeModel>();

                while (reader.Read())
                {
                    RecipeModel r = new RecipeModel();
                    r.RecipeName = reader.GetString(1);
                    r.RecipeCuisineType = reader.GetString(2);
                    r.RecipeCookingTime = reader.GetString(3);
                    r.RecipeIngredients = reader.GetString(4);
                    r.RecipeMethod = reader.GetString(5);
                    r.File = reader.GetString(6);
                    RecipeList.Add(r);
                }
                reader.Close();
                if (RecipeList.Count == 0)
                {
                    RecipeModel r = new RecipeModel();
                    r.RecipeName = "No Recipes Could Be Found.";
                    RecipeList.Add(r);
                }
            }
        }
    }
}
