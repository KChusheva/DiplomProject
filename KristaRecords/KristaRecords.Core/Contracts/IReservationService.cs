using KristaRecords.Infrastructure.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Contracts
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAll();

        Task<List<Reservation>> GetReservationsForUser(string userId);

        Task<bool> AddReservation(int scheduleId, string userId, int categoryId, int duration, TimeSpan fromHour, TimeSpan toHour, decimal totalPrice);
        List<(int, int)> GetReservationsDatesForSchedule(ICollection<Reservation> reservations);

        Task<Reservation> GetReservation(int id);

        Task DeleteReservation(int id);
    }
}
