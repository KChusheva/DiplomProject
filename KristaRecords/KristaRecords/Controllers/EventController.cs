using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KristaRecords.Core.Contracts;
using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Models.Category;
using KristaRecords.Models.Event;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;

namespace KristaRecords.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICategoryService _categoryService;

        public EventController(IEventService _eventervice, ICategoryService categoryService)
        {
            this._eventService = _eventervice;
            this._categoryService = categoryService;
        }

        // GET: EventController
        [AllowAnonymous]
        public ActionResult Index(string searchStringEventName, string searchStringCategoryName)
        {
            List<EventIndexVM> _event = _eventService.GetEvents(searchStringEventName, searchStringCategoryName)
            .Select(e => new EventIndexVM
            {
                Id = e.Id,
                EventName = e.EventName,
                Description = e.Description,
                Image = e.Image,
                CompletionDate = e.CompletionDate.ToString("MM/dd/yyyy HH:mm tt", CultureInfo.InvariantCulture),
                CategoryId = e.CategoryId,
                CategoryName = e.Category.CategoryName,
                CategoryHourlyRate = e.Category.HourlyRate
            }).ToList();
            return this.View(_event);

        }

        [AllowAnonymous]
        //GET: EventController/Details/5
        public ActionResult Details(int id)
        {
            Event item = _eventService.GetEventById(id);
            if (item == null)
            {
                return NotFound();
            }

            EventDetailsVM _event = new EventDetailsVM()
            {
                Id = item.Id,
                EventName = item.EventName,
                Description = item.Description,
                Image = item.Image,
                CompletionDate = item.CompletionDate.ToString("MM/dd/yyyy HH:mm tt", CultureInfo.InvariantCulture),
                CategoryName = item.Category.CategoryName,
                CategoryHourlyRate = item.Category.HourlyRate
            };

            return View(_event);
        }

        // GET: EventController/Create
        public ActionResult Create()
        {
            var _event = new EventCreateVM();
            _event.Categories = _categoryService.GetCategories()
               .Select(x => new CategoryPairVM()
               {
                   Id = x.Id,
                   Name = x.CategoryName,
                   HourlyRate = x.HourlyRate
               }).ToList();

            return View(_event);
        }

        // POST: EventController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] EventCreateVM _event)
        {
            if (ModelState.IsValid)
            {
                var parsedCompletionDate = DateTime.ParseExact(_event.CompletionDate, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                var createdId = _eventService.Create(_event.EventName, _event.Description, _event.Image, parsedCompletionDate, _event.CategoryId);
                if (createdId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        //GET: EventController/Edit/5
        public ActionResult Edit(int id)
        {
            Event _event = _eventService.GetEventById(id);
            if (_event == null)
            {
                return NotFound();
            }

            EventEditVM updatedEvent = new EventEditVM()
            {
                Id = _event.Id,
                EventName = _event.EventName,
                Description = _event.Description,
                Image = _event.Image,
                CompletionDate = _event.CompletionDate.ToString("MM/dd/yyyy HH:mm tt", CultureInfo.InvariantCulture),
                CategoryId = _event.CategoryId,
            };
           

            updatedEvent.Categories = _categoryService.GetCategories()
               .Select(c => new CategoryPairVM()
               {
                   Id = c.Id,
                   Name = c.CategoryName,
                   HourlyRate = c.HourlyRate
               })
               .ToList();

            return View(updatedEvent);
        }

        // POST: EventController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventEditVM _event)
        {
            {
                if (ModelState.IsValid)
                {
                    var parsedCompletionDate = DateTime.ParseExact(_event.CompletionDate, "MM/dd/yyyy h:mm tt", CultureInfo.InvariantCulture);
                    var updated = _eventService.Update(id, _event.EventName, _event.Description, _event.Image, parsedCompletionDate, _event.CategoryId);
                    if (updated)
                    {
                        return this.RedirectToAction("Index");
                    }

                }

                return View(_event);
            }
        }

        // GET: EventController/Delete/5
        public ActionResult Delete(int id)
        {
            Event item = _eventService.GetEventById(id);
            if (item == null)
            {
                return NotFound();
            }

            EventDeleteVM _event = new EventDeleteVM()
            {
                Id = item.Id,
                EventName = item.EventName,
                Description = item.Description,
                Image = item.Image,
                CompletionDate = item.CompletionDate.ToString("MM/dd/yyyy HH:mm tt", CultureInfo.InvariantCulture),
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                CategoryHourlyRate = item.Category.HourlyRate
            };

            return View(_event);
        }

        // POST: EventController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _eventService.RemoveById(id);

            if (deleted)
            {
                return this.RedirectToAction("Success");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
