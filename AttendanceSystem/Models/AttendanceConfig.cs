namespace AttendanceSystem.Models
{
    public class AttendanceConfig
    {
        public int Id { get; set; }
        public  DateTime Max_punch_in_time { get; set; }
        public DateTime Max_punch_out_time { get; set; }
    }
}
