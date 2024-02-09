using KristaRecords.Core.Contracts;
using KristaRecords.Core.Services;
using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Models.Category;
using KristaRecords.Models.Event;
using KristaRecords.Models.Reservation;
using KristaRecords.Models.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace KristaRecords.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            this._scheduleService = scheduleService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(bool isDiscounted, bool isAvailable)
        {
            IEnumerable<Schedule> schedules;

            if (isDiscounted == true && isAvailable == true)
            {
                schedules = await _scheduleService.GetAllDiscountedAndFreeSchedules();
            }
            else if (isAvailable == true)
            {
                schedules = await _scheduleService.GetAllFreeSchedules();
            }
            else if (isDiscounted == true)
            {
                schedules = await _scheduleService.GetAllDiscountedSchedules();
            }
            else
            {
                schedules = await _scheduleService.GetAll(); 
            }

            if ((!this.User.IsInRole("Administrator")))
            {
                schedules = schedules.Where(x => x.Date >= DateTime.Today && x.AvailableHours > 0);
            }

            IEnumerable<ScheduleIndexVM> schedulesVM = schedules.Select(x => new ScheduleIndexVM
            {
                Id = x.Id,
                AvailableHours = x.AvailableHours,
                Date = x.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Discount = x.Discount,
                IsBusy = x.IsBusy
            });

            return View(schedulesVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ScheduleCreateVM scheduleVM)
        {
            if (await _scheduleService.IsDateOccupied(scheduleVM.Date))
            {
                ModelState.AddModelError(nameof(scheduleVM.Date), "Schedule with this date alreay exists");
            }

            if (ModelState.IsValid)
            {
                var parsedDate = DateTime.ParseExact(scheduleVM.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                var created = await _scheduleService.AddSchedule(parsedDate, scheduleVM.AvailableHours, scheduleVM.Discount, false);
                if (created)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            Schedule schedule = await _scheduleService.GetSchedule(id);
            if (schedule == null)
            {
                return NotFound();
            }

            ScheduleEditVM updatedSchedule = new ScheduleEditVM()
            {
                Id = schedule.Id,
                Date = schedule.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableHours = schedule.AvailableHours,
                Discount = schedule.Discount,
                IsBusy = schedule.IsBusy,
            };

            return View(updatedSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ScheduleEditVM schedule)
        {
            if (ModelState.IsValid)
            {
                var parsedDate = DateTime.ParseExact(schedule.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var updated = await _scheduleService.UpdateSchedule(id, parsedDate, schedule.AvailableHours, schedule.Discount, schedule.IsBusy);

                if (updated)
                {
                    return this.RedirectToAction("Index");
                }

            }

            return View(schedule);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Schedule item = await _scheduleService.GetSchedule(id);
            if (item == null)
            {
                return NotFound();
            }

            ScheduleDeleteVM schedule = new ScheduleDeleteVM()
            {
                Id = item.Id,
                Date = item.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableHours = item.AvailableHours,
                Discount = item.Discount
            };

            return View(schedule);
        }

        // POST: EventController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var schedule = await _scheduleService.GetSchedule(id);

            ScheduleDeleteVM scheduleVM = new ScheduleDeleteVM()
            {
                Id = schedule.Id,
                Date = schedule.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableHours = schedule.AvailableHours,
                Discount = schedule.Discount
            };

            if (schedule == null)
            {
                return NotFound();
            }

            if (schedule.Reservations.Any())
            {
                ModelState.AddModelError(nameof(schedule.Reservations), "Cannot delete schedule because it has associated reservations.");
                return View(scheduleVM);
            }

            if (schedule.IsBusy)
            {
                ModelState.AddModelError(nameof(schedule.IsBusy), "Cannot delete schedule because it is occupied.");
                return View(scheduleVM);
            }

            var deleted = await _scheduleService.DeleteSchedule(id);

            if (deleted)
            {
                return RedirectToAction("Success");
            }
            else
            {
                return View(scheduleVM);
            }

        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            Schedule item = await _scheduleService.GetSchedule(id);

            if (item == null)
            {
                return NotFound();
            }

            ScheduleDetailsVM schedule = new ScheduleDetailsVM()
            {
                Id = item.Id,
                Date = item.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableHours = item.AvailableHours,
                Discount = item.Discount,
                IsBusy = item.IsBusy,
                Reservations = item.Reservations.Select(x => new ReservationIndexVM { })
            };

            return View(schedule);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
