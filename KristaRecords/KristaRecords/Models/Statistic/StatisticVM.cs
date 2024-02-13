using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Statistic
{
    public class StatisticVM
    {
        [Display(Name = "Count Clients")]
        public int CountClients { get; set; }
        [Display(Name = "Count Events")]
        public int CountEvents { get; set; }
        [Display(Name = "Count Reservations")]
        public int CountReservations { get; set; }
        [Display(Name = "Total Sum Reservations")]
        public decimal SumReservations { get; set; }

        [Display(Name = "Top Client")]
        public string? Username { get; set; }

        [Display(Name = "Top Client Reservations Count")]
        public int UserReservationCount { get; set; }
    }
}
