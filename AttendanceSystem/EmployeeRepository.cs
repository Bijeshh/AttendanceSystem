using AttendanceSystem.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
namespace AttendanceSystem
{
    public class EmployeeRepository
    {
        private readonly string ConnectionString;

        public EmployeeRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Employee> GetAllEmployee()
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.Query<Employee>("SELECT * FROM Employee").ToList();
        }

        public Employee GetEmployeeById(int? id)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.QueryFirstOrDefault<Employee>("SELECT * FROM Employee WHERE Id = @Id", new { Id = id });
        }

        public bool CreateEmployee(Employee employee)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            string query = "INSERT INTO Employee (Name) VALUES (@Name)";
            int rowsAffected = db.Execute(query, employee);
            return rowsAffected > 0;
        }

        public bool UpdateEmployee(Employee employee)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            string query = "UPDATE Employee SET Name = @Name  WHERE Id = @Id";
            int rowsAffected = db.Execute(query, employee);
            return rowsAffected > 0;
        }

        public bool DeleteEmployee(int id)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            string query = "DELETE FROM Employee WHERE Id = @Id";
            int rowsAffected = db.Execute(query, new { Id = id });
            return rowsAffected > 0;
        }

        public bool EmployeeExists(int id)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            string query = "SELECT COUNT(*) FROM Employee WHERE Id = @Id";
            int count = db.ExecuteScalar<int>(query, new { Id = id });
            return count > 0;
        }
    }
}