﻿namespace KristaRecords.Models.Reservation
{
    public class ReservationIndexVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? ScheduleDate { get; set; }

        public int DurationHours { get; set; }
        public string? CategoryName { get; set; }
        public string? FromHour { get; set; }

        public string? ToHour { get; set; }

        public decimal HourlyRate { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalPrice { get; set; }

        public string? SubmissionDate { get; set; }
    }
}
