using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AttendanceSystem.Data;
using AttendanceSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
namespace AttendanceSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeRepository employeeRepository;
        private readonly IHttpContextAccessor? contxt;

        public EmployeesController(IHttpContextAccessor httpContextAccessor)
        {
            string connectionString = "Server=.\\SQLEXPRESS; Database=AttendanceDB; User ID=; Password=; Trusted_Connection=True; MultipleActiveResultSets=true; TrustServerCertificate=true;";
            employeeRepository = new EmployeeRepository(connectionString);
            contxt = httpContextAccessor;
        }

        // GET: Employee
        public IActionResult Index()
        {
            List<Employee> employee = employeeRepository.GetAllEmployee();
            return View(employee);
        }

        // GET: Employee/Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public IActionResult Create([Bind("Id,Name")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                bool created = employeeRepository.CreateEmployee(employee);
                if (created)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }

        // GET: Employee/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Name")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool updated = employeeRepository.UpdateEmployee(employee);

                if (updated)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employee);
        }


        // GET: Employee/Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = employeeRepository.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = employeeRepository.DeleteEmployee(id);

            if (deleted)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }
}