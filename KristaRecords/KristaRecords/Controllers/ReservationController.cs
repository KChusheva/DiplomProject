using KristaRecords.Core.Contracts;
using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Infrastrucutre.Data;
using KristaRecords.Models.Category;
using KristaRecords.Models.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace KristaRecords.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IReservationService _reservationService;
        private readonly ICategoryService _categoryService;

        public ReservationController(IScheduleService scheduleService, IReservationService reservationService, ICategoryService categoryService)
        {
            _scheduleService = scheduleService;
            _reservationService = reservationService;
            _categoryService = categoryService;
        }
        
        public async Task<IActionResult> Create(int id)
        {
            Schedule schedule = await _scheduleService.GetSchedule(id);

            if (schedule == null || (schedule?.IsBusy ?? true))
            {
                return NotFound();
            }

            ReservationCreateVM reservation = new ReservationCreateVM();
            reservation.Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM()
            {
                Id = x.Id,
                Name = x.CategoryName,
                HourlyRate = x.HourlyRate
            }).ToList();

            reservation.ScheduleDate = schedule.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            reservation.Discount = schedule.Discount;

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, ReservationCreateVM bindingModel)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            TimeSpan fromHour = DateTime.ParseExact(bindingModel.FromHour, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            TimeSpan toHour = DateTime.ParseExact(bindingModel.ToHour, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

            if (fromHour >= toHour || bindingModel.Duration <= 0)
            {
                this.ModelState.AddModelError("", "Invalid period");
            }

            if (!ModelState.IsValid)
            {
                return this.View(bindingModel);
            }

            var created = await _reservationService.AddReservation(id, currentUserId, bindingModel.CategoryId, bindingModel.Duration, fromHour, toHour, bindingModel.TotalPrice);

            if(created)
            {
                return RedirectToAction("Index", "Schedule");
            }

            return View(bindingModel);
        }
    }
}
