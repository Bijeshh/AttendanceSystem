using AttendanceSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using NuGet.DependencyResolver;
using NuGet.Protocol.Plugins;
using System.Runtime;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttendanceSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly string ConnectionString = "Server=.\\SQLEXPRESS; Database=AttendanceDB; User ID=your_user_id; Password=your_password; Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=true;";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Report()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sqlQuery = "SELECT ac.Id, ac.Max_punch_in_time, ac.Max_punch_out_time, emp.Id AS Employee_Id, ad.Punch_in_time, ad.Punch_out_time,emp.Name FROM AttendanceConfig ac, AttendanceDetail ad,Employee emp WHERE ac.Id = ad.Id and ad.Id=emp.Id ORDER By emp.Id ASC";
                var result = db.QueryFirstOrDefault(sqlQuery);

                int Id = result.Id;
                DateTime? PunchInTime = result.Punch_in_time;
                DateTime? PunchOutTime = result.Punch_out_time;
                DateTime? MaxPunchInTime = result.Max_punch_in_time;
                DateTime? MaxPunchOutTime = result.Max_punch_out_time;
                int Employee_Id = result.Employee_Id;
                string Name = result.Name;

                ViewBag.Id = Id;
                ViewBag.Employee_Id = Employee_Id;
                ViewBag.Name = Name;
                ViewBag.MaxPunchInTime = MaxPunchInTime;
                ViewBag.MaxPunchOutTime = MaxPunchOutTime;
                ViewBag.PunchInTime = PunchInTime;
                ViewBag.PunchOutTime = PunchOutTime;
            }
            return View("Report");
        }

        public IActionResult AdminConfigure()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminConfigure(DateTime MaxPunchInTime ,DateTime MaxPunchOutTime )
        {  
            DateTime MaxpunchInDateTime = MaxPunchInTime ;
            DateTime MaxpunchOutDateTime = MaxPunchOutTime;
            using IDbConnection db = new SqlConnection(ConnectionString);
            {
                string sqlQuery = "INSERT INTO AttendanceConfig (Max_punch_in_time, Max_punch_out_time) VALUES (@MaxpunchInDateTime, @MaxpunchOutDateTime)";
                db.Execute(sqlQuery, new { MaxpunchInDateTime, MaxpunchOutDateTime });
            }
                return RedirectToAction("AttendanceConfigDetails");
        }

        public IActionResult AttendanceConfigDetails()
        {
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    string sqlQuery = "SELECT TOP 1 Max_punch_in_time, Max_punch_out_time FROM AttendanceConfig ORDER BY Id DESC";
                    var result = db.QueryFirstOrDefault(sqlQuery);

                    DateTime MaxPunchInTime = result.Max_punch_in_time;
                    DateTime MaxPunchOutTime = result.Max_punch_out_time;

                    ViewBag.MaxPunchInTime = MaxPunchInTime;
                    ViewBag.MaxPunchOutTime = MaxPunchOutTime;

 
                    if (DateTime.Now < MaxPunchInTime && DateTime.Now > MaxPunchOutTime)
                    {
                        return RedirectToAction("EmpAbout", "Home");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
 


        [HttpPost]
        public IActionResult PunchInTime()
        {
            DateTime maxPunchInTime;
            DateTime currentTime = DateTime.Now;

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                DateTime? punchInTime = currentTime;
                string sqlQuery = "INSERT INTO AttendanceDetail (Punch_in_time) VALUES (@PunchInTime)";
                db.Execute(sqlQuery, new { PunchInTime = punchInTime });

                string query = "SELECT TOP 1 Max_punch_in_time FROM AttendanceConfig";
                var result = db.QueryFirstOrDefault(query);
                maxPunchInTime = result.Max_punch_in_time;
            }
            return RedirectToAction("Status");
        }

        [HttpPost]
        public IActionResult PunchOutTime()
        {
            DateTime maxPunchOutTime;
            DateTime currentTime = DateTime.Now;

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                DateTime? punchOutTime = currentTime;
                string sqlQuery = "INSERT INTO AttendanceDetail (Punch_out_time) VALUES (@PunchOutTime)";
                db.Execute(sqlQuery, new { PunchOutTime=punchOutTime });

                string query = "SELECT TOP 1 Max_punch_out_time FROM AttendanceConfig ORDER BY Id DESC";
                var result = db.QueryFirstOrDefault(query);
                maxPunchOutTime = result.Max_punch_out_time;
            }
            return RedirectToAction("Status");
        }

        public IActionResult Status()
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            {
                string sqlQuery="Select Top 1 Id,Punch_in_time,Punch_out_time from AttendanceDetail order by Id desc";
                var result = db.QueryFirstOrDefault(sqlQuery);
                int Id = result.Id;
                DateTime? PunchInTime = result.Punch_in_time;
                DateTime? PunchOutTime = result.Punch_out_time;
                ViewBag.Id = Id;
                ViewBag.PunchInTime = PunchInTime;
                ViewBag.PunchOutTime = PunchOutTime;
            }
            return View("Status");
        }

        public IActionResult EarlyPunchOutReason(string reason)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string sqlQuery = "INSERT INTO AttendanceDetail (Early_punch_out_reason) VALUES (@Reason)";
                db.Execute(sqlQuery, new { Reason = reason });
            }
            return View("Index");
        }

        public IActionResult LatePunchInReason(string reason)
        {
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    string sqlQuery = "INSERT INTO AttendanceDetail (Late_punch_in_reason) VALUES (@Reason)";
                    db.Execute(sqlQuery, new { Reason = reason });
                }
                return View("Index");
        }
    }
}
