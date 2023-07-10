using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanceSystem.Models;

namespace AttendanceSystem.Data
{
    public class AttendanceSystemContext : DbContext
    {
        public AttendanceSystemContext (DbContextOptions<AttendanceSystemContext> options)
            : base(options)
        {
        }

        public DbSet<AttendanceSystem.Models.Employee> Employee { get; set; } = default!;
    }
}
