using KristaRecords.Core.Contracts;
using KristaRecords.Infrastructure.Data.Domain;
using KristaRecords.Infrastrucutre.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Services
{
    public class EventService : IEventService
    {

        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(string name, string description, string image, DateTime completionDate, int categoryId)
        {
            Event item = new Event
            {
                EventName = name,
                Description = description,
                Image = image,
                CompletionDate = completionDate,
                Category = _context.Categories.Find(categoryId),
            };

            _context.Events.Add(item);
            return _context.SaveChanges() != 0;
        }

        public Event GetEventById(int eventId)
        {
            return _context.Events.Find(eventId);
        }

        public List<Event> GetEvents()
        {
            List<Event> events = _context.Events.ToList();
            return events;
        }

        public List<Event> GetEvents(string searchStringEventName, string searchStringCategoryName)
        {
            List<Event> events = _context.Events.ToList();
            if (!String.IsNullOrEmpty(searchStringEventName) && !String.IsNullOrEmpty(searchStringCategoryName))
            {
                events = events.Where(x => x.EventName.ToLower().Contains(searchStringEventName.ToLower())
                && x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringEventName))
            {
                events = events.Where(x => x.EventName.ToLower().Contains(searchStringEventName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringCategoryName))
            {
                events = events.Where(x => x.Category.CategoryName.ToLower().Contains(searchStringCategoryName.ToLower())).ToList();
            }

            return events;
        }

        public bool RemoveById(int eventId)
        {
            var _event = GetEventById(eventId);

            if (_event == default(Event))
            {
                return false;
            }

            _context.Remove(_event);
            return _context.SaveChanges() != 0;
        }

        public bool Update(int eventId, string name, string description, string image, DateTime completionDate, int categoryId)
        {
            var _event = GetEventById(eventId);
            if (_event == default(Event))
            {
                return false;
            }

            _event.EventName = name;
            _event.Description = description;
            _event.Image = image;
            _event.CompletionDate = completionDate;
            _event.Category = _context.Categories.Find(categoryId);

            _context.Update(_event);
            return _context.SaveChanges() != 0;
        }
    }
}
