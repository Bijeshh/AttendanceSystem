using System.Security.Cryptography.X509Certificates;

namespace AttendanceSystem.Models
{
    public class AttendanceDetail
    {
        public int Id { get; set; } 
        public int Employee_Id { get; set; }
        public DateTime Punch_in_time { get; set; } 
        public DateTime Punch_out_time { get; set; }
        public String? Late_punch_in_reason { get; set; } 
        public string? Early_punch_out_reason { get; set; }      
    }
}
    