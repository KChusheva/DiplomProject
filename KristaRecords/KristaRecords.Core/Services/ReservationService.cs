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

            if (duration > schedule.AvailableHours)
            {
                return false;
            }

            schedule.AvailableHours -= duration;
            schedule.IsBusy = true;
            _context.Schedules.Update(schedule);
            _context.Reservations.Add(reservation);

            return await _context.SaveChangesAsync() != 0;

        }

        public async Task<List<Reservation>> GetAll()
        {
            return await _context.Reservations.OrderByDescending(x => x.Schedule.Date).ThenBy(x => x.FromHour).ToListAsync();
        }

        public async Task<Reservation> GetReservation(int id)
        {
            return await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<(int, int)> GetReservationsDatesForSchedule(ICollection<Reservation> reservations)
        {
            var reservationsForSchedule = reservations.Select(r => (r.FromHour.Hours, r.ToHour.Hours)).ToList();

            return reservationsForSchedule;
        }

        public async Task<List<Reservation>> GetReservationsForUser(string userId)
        {
            return await _context.Reservations.Where(x => x.UserId == userId).OrderByDescending(x => x.SubmissionDateTime).ToListAsync();
        }

        public async Task DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();  
            }
        }
    }
}
