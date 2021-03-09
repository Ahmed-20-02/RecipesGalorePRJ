using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipesGalorePRJ.Models;
using RecipesGalorePRJ.Pages.DBConnection;

namespace RecipesGalorePRJ.Pages.Guest
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public UserModel user { get; set; }
        public DatabaseConnection connection;
        public IActionResult OnPost()
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO UserDBO (FirstName, EmailAddress, Password) VALUES (@FName, @Email, @Pwd)";

                command.Parameters.AddWithValue("@FName", user.FirstName);
                command.Parameters.AddWithValue("@Email", user.EmailAddress);
                command.Parameters.AddWithValue("@Pwd", user.Password);

                command.ExecuteNonQuery();
            }

            return RedirectToPage("/Guest/Login");
        }

        public void OnGet()
        {
        }
    }
}
