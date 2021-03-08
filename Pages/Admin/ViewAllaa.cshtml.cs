using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipesGalorePRJ.Models;

namespace RecipesGalorePRJ.Pages.Admin
{
    public class ViewAllaaModel : PageModel
    {
        public List<Recipe> RecipeList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FLTR { get; set; }

        public List<string> FilterType { get; set; } = new List<string> { "Halal", "Vegetarian", "Vegan" };

        public void OnGet()
        {
            ViewRecipes();
        }

        public void ViewRecipes()
        {
            string DbConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=7b/cDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                if (string.IsNullOrEmpty(FLTR) || FLTR == "All")
                {
                    command.CommandText = @"SELECT * FROM Recipes";
                }
                else
                {
                    command.CommandText += @"SELECT * FROM Recipes WHERE FilterType = @FilterT";
                    command.Parameters.AddWithValue("@FilterT", FLTR);
                }

                SqlDataReader reader = command.ExecuteReader(); //SqlDataReader is used to read record from a table

                RecipeList = new List<Recipe>(); //this object of list is created to populate all records from the table

                while (reader.Read())
                {
                    Recipe r = new Recipe();
                    r.RecipeName = reader.GetString(1); //getting the second field from the table
                    r.File = reader.GetString(7);
                    RecipeList.Add(r);
                }
                reader.Close();
            }
        }
    }
}
