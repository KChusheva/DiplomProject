using KristaRecords.Core.Contracts;
using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Infrastrucutre.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Humanizer.In;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace KristaRecords.Core.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ScheduleService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Schedule>> GetAll()
        {
            return await _context.Schedules.ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetAllDiscountedSchedules()
        {
            return await _context.Schedules.Where(x => x.Discount > 0).ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetAllFreeSchedules()
        {
            return await _context.Schedules.Where(x => x.IsBusy != true).ToListAsync();
        }
        public async Task<IEnumerable<Schedule>> GetAllDiscountedAndFreeSchedules()
        {
            return await _context.Schedules.Where(x => x.IsBusy != true && x.Discount > 0).ToListAsync();
        }

        public async Task<Schedule> GetSchedule(int id)
        {
            return await _context.Schedules.Include(s => s.Reservations)
                                           .ThenInclude(x => x.Category)
                                           .Include(s => s.Reservations)
                                           .ThenInclude(x => x.User)
                                           .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> AddSchedule(DateTime date, int availableHours, decimal discount, bool isBusy = false)
        {
            Schedule schedule = new Schedule
            {
                Date = date,
                AvailableHours = availableHours,
                Discount = discount,
                IsBusy = isBusy
            };

            await _context.Schedules.AddAsync(schedule);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateSchedule(int id, DateTime date, int availableHours, decimal discount, bool isBusy = false)
        {
            var schedule = await GetSchedule(id);

            if (schedule == default(Schedule))
            {
                return false;
            }

            if (!schedule.Reservations.Any() || IsBusyOnlyChanged(schedule, isBusy, date, availableHours, discount))
            {

                schedule.AvailableHours = availableHours;
                schedule.Discount = discount;
                schedule.IsBusy = isBusy;
                schedule.Date = date;

                _context.Update(schedule);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private bool IsBusyOnlyChanged(Schedule schedule, bool newIsBusy, DateTime newDate, int newAvailableHours, decimal newDiscount)
        {
            return schedule.IsBusy != newIsBusy &&
                   schedule.Date == newDate &&
                   schedule.AvailableHours == newAvailableHours &&
                   schedule.Discount == newDiscount;
        }

        public async Task<bool> DeleteSchedule(int id)
        {
            var schedule = await GetSchedule(id);

            if (schedule == default(Schedule))
            {
                return false;
            }

            _context.Remove(schedule);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> IsDateOccupied(string date)
        {
            return await _context.Schedules
                                 .AnyAsync(x =>
                                           x.Date.Year == DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture).Year &&
                                           x.Date.Month == DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture).Month &&
                                           x.Date.Day == DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture).Day
                                          );
        }

        public async Task<List<DateTime>> GetAllOccupiedDates()
        {
            return await _context.Schedules.Select(x => x.Date).ToListAsync();
        }
    }
}
