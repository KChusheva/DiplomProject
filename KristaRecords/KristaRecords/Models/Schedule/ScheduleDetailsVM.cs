﻿using KristaRecords.Models.Reservation;
using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Schedule
{
    public class ScheduleDetailsVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Date { get; set; }

        [Required]
        [Range(1, 12)]
        [Display(Name = "Available Hours")]
        public int AvailableHours { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Discount { get; set; }

        [Display(Name = "Schedule is busy?")]
        public bool IsBusy { get; set; }
        public IEnumerable<ReservationScheduleVM> Reservations { get; set; }
    }
}
