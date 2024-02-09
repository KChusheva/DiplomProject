using KristaRecords.Infrastructure.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Contracts
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAll();
        Task<IEnumerable<Schedule>> GetAllFreeSchedules();
        Task<IEnumerable<Schedule>> GetAllDiscountedSchedules();
        Task<IEnumerable<Schedule>> GetAllDiscountedAndFreeSchedules();
        Task<Schedule> GetSchedule(int id);

        Task<bool> AddSchedule(DateTime date, int availableHours, decimal discount, bool isBusy = false);
        Task<bool> UpdateSchedule(int id, DateTime date, int availableHours, decimal discount, bool isBusy = false);
        Task<bool> DeleteSchedule(int id);

        Task<bool> IsDateOccupied(string date);

        Task<List<DateTime>> GetAllOccupiedDates();
    }
}
