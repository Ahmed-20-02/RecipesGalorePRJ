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
                command.CommandText = @"SELECT UserID, FirstName, EmailAddress, Password FROM UserDBO WHERE EmailAddress = @Email AND Password = @Pwd";

                command.Parameters.AddWithValue("@Email", user.EmailAddress);
                command.Parameters.AddWithValue("@Pwd", user.Password);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.UserID = reader.GetInt32(0);
                    user.FirstName = reader.GetString(1);
                    user.EmailAddress = reader.GetString(2);
                    user.Password = reader.GetString(3);
                }

                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    SessionID = HttpContext.Session.Id;
                    HttpContext.Session.SetString("sessionID", SessionID);
                    HttpContext.Session.SetInt32("id", user.UserID);
                    HttpContext.Session.SetString("fname", user.FirstName);

                    if (user.EmailAddress == "admin@recipegalore.com")
                    {
                        return RedirectToPage("/Admin/AdminViewAllRecipes");
                    }
                    else
                    {
                        return RedirectToPage("/User/UserViewAllRecipes");
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
