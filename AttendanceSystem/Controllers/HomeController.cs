using AttendanceSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using MongoDB.Driver.Core.Configuration;

namespace LoginApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string ConnectionString = "Server=.\\SQLEXPRESS; Database=AttendanceDB; User ID=your_user_id; Password=your_password; Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=true;";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both username and password.";
                return View();
            }

            if (IsValidUser(username, password))
            {
                HttpContext.Session.SetString("Username", username);
                if (username == "admin" && password == "admin")
                {
                    return RedirectToAction("About", "Home");
                }
                else
                {
                    return RedirectToAction("AttendanceConfigDetails", "Attendance");
                }
            }
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        private bool IsValidUser(string username, string password)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            string sqlQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
            var count = db.ExecuteScalar<int>(sqlQuery, new { Username = username, Password = password });
            return count > 0;
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }

        /* public IActionResult Register(string username, string password)
         {
             using IDbConnection db = new SqlConnection(ConnectionString);
             string sqlQuery = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
             db.Execute(sqlQuery, new { Username = username, Password = password });
             return View("Index");
         }*/

        public IActionResult About()
        {
            return View("About");
        }

        public IActionResult EmpAbout()
        {
            return View("EmpAbout");
        }
    }
}
