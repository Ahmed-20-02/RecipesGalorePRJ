using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipesGalorePRJ.Models;
using RecipesGalorePRJ.Pages.DBConnection;

namespace RecipesGalorePRJ.Pages.Guest
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public UserModel user { get; set; }
        public DatabaseConnection connection;

        public string Message { get; set; }

        public string SessionID;

        public IActionResult OnPost()
        {
            connection = new DatabaseConnection();
            string DbConnection = connection.DatabaseConn();

            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT Id, FirstName, EmailAddress, Password FROM UserDBO WHERE EmailAddress = @Email AND Password = @Pwd";

                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Pwd", user.Password);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.Id = reader.GetInt32(0);
                    user.Name = reader.GetString(1);
                    user.Email = reader.GetString(2);
                    user.Password = reader.GetString(3);
                }

                if (!string.IsNullOrEmpty(user.Name))
                {
                    SessionID = HttpContext.Session.Id;
                    HttpContext.Session.SetString("sessionID", SessionID);
                    HttpContext.Session.SetInt32("id", user.Id);
                    HttpContext.Session.SetString("fname", user.Name);

                    if (user.Email == "admin@recipegalore.com")
                    {
                        return RedirectToPage("/Admin/AdminViewAllRecipes");
                    }
                    else
                    {
                        return RedirectToPage("/User/ViewAllRecipes");
                    }
                }
                else
                {
                    Message = "Invalid Username and Password!";
                    return Page();
                }
            }
        }
        public void OnGet()
        {
        }
    }
}
