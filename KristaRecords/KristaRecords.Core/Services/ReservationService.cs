using KristaRecords.Core.Contracts;
using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Infrastrucutre.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> AddReservation(int scheduleId, string userId, int categoryId, int duration, TimeSpan fromHour, TimeSpan toHour, decimal totalPrice)
        {
            var schedule = await _context.Schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
            var category = await _context.Categories.FindAsync(categoryId);

            if (schedule == null)
            {
                return false;
            }

            Reservation reservation = new Reservation
            {
                Schedule = schedule,
                DurationHours = duration,
                Category = category,
                UserId = userId,
                HourlyRate = category.HourlyRate,
                Discount = schedule.Discount,
                FromHour = fromHour,
                ToHour = toHour,
                TotalAmount = totalPrice
            };

            if(duration > schedule.AvailableHours)
            {
                return false;
            }

            schedule.AvailableHours -= duration;
            schedule.IsBusy = true; 
            _context.Schedules.Update(schedule);
            _context.Reservations.Add(reservation);

            return await _context.SaveChangesAsync() != 0;

        }

        public Task<IEnumerable<T>> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetReservationsForUser<T>(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
