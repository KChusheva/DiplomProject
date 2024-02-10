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
        public Task<IEnumerable<T>> GetAll<T>();

        public Task<IEnumerable<T>> GetReservationsForUser<T>(string userId);

        Task<bool> AddReservation(int scheduleId, string userId, int categoryId, int duration, TimeSpan fromHour, TimeSpan toHour, decimal totalPrice);
    }
}
