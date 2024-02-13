using KristaRecords.Infrastructure.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Contracts
{
    public interface IStatisticService
    {
        int CountEvents();
        int CountClients();
        int CountReservations();
        decimal SumReservations();
        (ApplicationUser User, int ReservationCount)? GetTopClient();
    }
}
