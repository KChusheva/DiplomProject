using KristaRecords.Core.Contracts;
using KristaRecords.Core.Services;
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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(string Username)
        {
            List<Reservation> reservationsDB = await _reservationService.GetAll();

            if (!String.IsNullOrEmpty(Username))
            {
                reservationsDB = reservationsDB.Where(x => x.User.UserName.ToLower().Contains(Username.ToLower())).ToList();
            }

            List<ReservationIndexVM> reservations = reservationsDB.Select(x => new ReservationIndexVM
            {
                Id = x.Id,
                Username = x.User.UserName,
                CategoryName = x.Category.CategoryName,
                Discount = x.Discount,
                DurationHours = x.DurationHours,
                ScheduleDate = x.Schedule.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                FromHour = x.FromHour.ToString("hh"),
                ToHour = x.ToHour.ToString("hh"),
                HourlyRate = x.HourlyRate,
                SubmissionDate = x.SubmissionDateTime.ToString("dd/MM/yyyy hh:mm"),
                TotalPrice = x.TotalAmount

            }).ToList();

            return View(reservations);
        }

        public async Task<IActionResult> MyReservations()
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ReservationIndexVM> reservations = (await _reservationService.GetReservationsForUser(currentUserId)).Select(x => new ReservationIndexVM
            {
                Id = x.Id,
                Username = x.User.UserName,
                CategoryName = x.Category.CategoryName,
                Discount = x.Discount,
                DurationHours = x.DurationHours,
                ScheduleDate = x.Schedule.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                FromHour = x.FromHour.ToString("hh"),
                ToHour = x.ToHour.ToString("hh"),
                HourlyRate = x.HourlyRate,
                SubmissionDate = x.SubmissionDateTime.ToString("dd/MM/yyyy hh:mm"),
                TotalPrice = x.TotalAmount
            }).ToList();

            return View(reservations);
        }

        public async Task<IActionResult> Create(int id)
        {
            Schedule schedule = await _scheduleService.GetSchedule(id);

            if (schedule == null || (schedule.Date < DateTime.Now) || schedule?.AvailableHours <= 0)
            {
                return NotFound();
            }

            ReservationCreateVM reservation = new ReservationCreateVM();

            if (schedule?.Reservations != null)
            {   
                reservation.ReservationsDates = _reservationService.GetReservationsDatesForSchedule(schedule.Reservations);
            }

            reservation.Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM()
            {
                Id = x.Id,
                Name = x.CategoryName,
                HourlyRate = x.HourlyRate
            }).ToList();

            reservation.ScheduleDate = schedule!.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            reservation.Discount = schedule!.Discount;
            reservation.AvailableHours = schedule!.AvailableHours;

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, ReservationCreateVM bindingModel)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            TimeSpan fromHour = DateTime.ParseExact(bindingModel.FromHour, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
            TimeSpan toHour = DateTime.ParseExact(bindingModel.ToHour, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

            Schedule schedule = await _scheduleService.GetSchedule(id);

            if (schedule == null || (schedule.Date < DateTime.Now) || schedule?.AvailableHours <= 0)
            {
                return NotFound();
            }

            if (schedule.Reservations != null)
            {
                bindingModel.ReservationsDates = _reservationService.GetReservationsDatesForSchedule(schedule.Reservations);
            }

            bindingModel.Categories = _categoryService.GetCategories().Select(x => new CategoryPairVM()
            {
                Id = x.Id,
                Name = x.CategoryName,
                HourlyRate = x.HourlyRate
            }).ToList();

            bindingModel.ScheduleDate = schedule.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            bindingModel.Discount = schedule.Discount;
            bindingModel.AvailableHours = schedule.AvailableHours;

            if (fromHour >= toHour || bindingModel.Duration <= 0 || bindingModel.Duration > schedule.AvailableHours)
            {
                this.ModelState.AddModelError("", "Invalid reservation period");
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

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationService.GetReservation(id);

            if (reservation == null)
            {
                this.NotFound();
            }

            await _reservationService.DeleteReservation(id);

            return this.RedirectToAction(nameof(Index));
        }
    }
}
