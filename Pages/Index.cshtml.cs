using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipesGalorePRJ.Models;
using RecipesGalorePRJ.Pages.DBConnection;

namespace RecipesGalorePRJ.Pages
{
    public class IndexModel : PageModel
    {
        public List<RecipeModel> RecipeList { get; set; }
        public DatabaseConnection connection;
        [BindProperty(SupportsGet = true)]
        public string FLTR { get; set; }
        public List<string> FilterType { get; set; } = new List<string> { "Halal", "Vegetarian", "Vegan" };

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string userID;
        public const string SessionKeyName1 = "id";

        public string FirstName;
        public const string SessionKeyName2 = "fname";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";

        public IActionResult OnGet()
        {
            userID = HttpContext.Session.GetString(SessionKeyName1);
            FirstName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);

            if (string.IsNullOrEmpty(userID) && string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Guest/Login");
            }
            return Page();
        }

        public void ViewRecipes()
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

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

                SqlDataReader reader = command.ExecuteReader();

                RecipeList = new List<RecipeModel>();

                while (reader.Read())
                {
                    RecipeModel rec = new RecipeModel();
                    rec.RecipeId = reader.GetInt32(0);
                    rec.RecipeName = reader.GetString(1);
                    rec.RecipeCuisineType = reader.GetString(2);
                    rec.RecipeCookingTime = reader.GetString(3);
                    rec.RecipeIngredients = reader.GetString(4);
                    rec.RecipeMethod = reader.GetString(5);
                    rec.File = reader.GetString(7);

                    RecipeList.Add(rec);
                }
                reader.Close();
            }
        }
    }
}
