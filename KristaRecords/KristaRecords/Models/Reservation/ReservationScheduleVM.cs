namespace KristaRecords.Models.Reservation
{
    public class ReservationScheduleVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? ScheduleDate { get; set; }

        public string? FromHour { get; set; }

        public string? ToHour { get; set; }

        public string? CategoryName { get; set; }

        public decimal TotalPrice { get; set; }

        public string? SubmissionDate { get; set; }
    }
}
