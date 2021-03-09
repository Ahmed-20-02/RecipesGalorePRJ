using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using RecipesGalorePRJ.Models;
using RecipesGalorePRJ.Pages.DBConnection;

namespace RecipesGalorePRJ.Pages.Admin
{
    public class AdminSearchRecipe : PageModel
    {

        public List<RecipeModel> RecipeList = new List<RecipeModel>();
        public DatabaseConnection connection;

        [BindProperty]
        public string SearchString { get; set; }

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
                    command.CommandText = @"SELECT * FROM RecipeDBO WHERE Name = t";
                }
                else
                {
                    command.CommandText = @"SELECT * FROM RecipeDBO WHERE (Name LIKE '%' + @SearchS) OR (Name LIKE @SearchS + '%')";
                    command.Parameters.AddWithValue("@SearchS", SearchString);
                }

                SqlDataReader reader = command.ExecuteReader();

                RecipeList = new List<RecipeModel>();

                while (reader.Read())
                {
                    RecipeModel r = new RecipeModel();
                    r.RecipeID = reader.GetInt32(0);
                    r.Name = reader.GetString(1);
                    r.CuisineType = reader.GetString(2);
                    r.CookingTime = reader.GetString(3);
                    r.Ingredients = reader.GetString(4);
                    r.Method = reader.GetString(5);
                    r.File = reader.GetString(6);
                    RecipeList.Add(r);
                }
                reader.Close();
                if (RecipeList.Count == 0)
                {
                    RecipeModel r = new RecipeModel();
                    r.Name = "Could Not Find Recipe";
                    RecipeList.Add(r);
                }
            }
        }
    }
}
