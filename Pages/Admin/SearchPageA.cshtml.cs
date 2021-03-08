using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using RecipesGalorePRJ.Models;

namespace RecipesGalorePRJ.Pages.Admin
{
    public class SearchPageAModel : PageModel
    {

        public List<Recipe> RecipeList = new List<Recipe>();

        [BindProperty]
        public string SearchString { get; set; }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            string DbConnection = @"";

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

                SqlDataReader reader = command.ExecuteReader(); //SqlDataReader is used to read record from a table

                RecipeList = new List<Recipe>(); //this object of list is created to populate all records from the table

                while (reader.Read())
                {
                    Recipe r = new Recipe();
                    r.RecipeName = reader.GetString(1); //getting the second field from the table
                    r.RecipeIngredients = reader.GetString(3);
                    r.RecipeMethod = reader.GetString(4);
                    r.RecipeLikes = reader.GetInt32(5);
                    r.FilterType = reader.GetString(6);
                    RecipeList.Add(r);
                }
                reader.Close();
                if (RecipeList.Count == 0)
                {
                    Recipe r = new Recipe();
                    r.RecipeName = "Could Not Find Recipe";
                    RecipeList.Add(r);
                }
            }
        }
    }
}
