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
            string DbConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=7b/cDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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
                    r.RecipeName = reader.GetString(2); //getting the second field from the table
                    r.File = reader.GetString(7);

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
