using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KristaRecords.Core.Contracts;
using KristaRecords.Infrastrucutre.Data;
using KristaRecords.Infrastructure.Data.Domain;

namespace KristaRecords.Core.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly ApplicationDbContext _context;

        public StatisticService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CountClients()
        {
            var adminRoleId = _context.Roles.Where(r => r.Name == "Administrator").Select(r => r.Id).FirstOrDefault();

            return _context.Users
            .Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
            .Count();
        }

        public int CountReservations()
        {
            return _context.Reservations.Count();
        }

        public int CountEvents()
        {
            return _context.Events.Count();
        }

        public decimal SumReservations()
        {
            var sum = _context.Reservations.Sum(x => x.TotalAmount);
            return sum;
        }

        public (ApplicationUser User, int ReservationCount)? GetTopClient()
        {
            var userWithHighestTotalAmountAndCount = _context.Reservations
            .GroupBy(r => r.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                TotalAmount = g.Sum(r => r.TotalAmount),
                ReservationCount = g.Count()
            })
            .OrderByDescending(x => x.TotalAmount)
            .FirstOrDefault();

            var userReservationsCount = 0;

            ApplicationUser user = null;

            if (userWithHighestTotalAmountAndCount != null)
            {
                user = _context.Users.FirstOrDefault(u => u.Id == userWithHighestTotalAmountAndCount.UserId);
                userReservationsCount = userWithHighestTotalAmountAndCount.ReservationCount;
            }


            return (user, userReservationsCount);
        }
    }
}

