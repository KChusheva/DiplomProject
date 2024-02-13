using KristaRecords.Core.Contracts;
using KristaRecords.Models.Statistic;
using Microsoft.AspNetCore.Mvc;

namespace KristaRecords.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IStatisticService _statisticsService;

        public StatisticController(IStatisticService statisticsService)
        {
            this._statisticsService = statisticsService;
        }
        public IActionResult Index()
        {
            StatisticVM statistics = new StatisticVM();

            statistics.CountClients = _statisticsService.CountClients();
            statistics.CountReservations = _statisticsService.CountReservations();
            statistics.SumReservations = _statisticsService.SumReservations();
            statistics.CountEvents = _statisticsService.CountEvents();

            var result = _statisticsService.GetTopClient();
            var username = " - ";
            
            if (result.Value.User != null)
            {
                username = result.Value.User.UserName;
            }

            statistics.Username = username;
            statistics.UserReservationCount = result.Value.ReservationCount;

            return View(statistics);

        }
    }
}
