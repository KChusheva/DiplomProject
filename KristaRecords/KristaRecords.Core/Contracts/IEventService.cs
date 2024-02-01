using KristaRecords.Infrastructure.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Contracts
{
    public interface IEventService
    {
        bool Create(string name, string description, string image, DateTime completionDate, int categoryId);

        bool Update(int eventId, string name, string description, string image, DateTime completionDate, int categoryId);

        List<Event> GetEvents();

        Event GetEventById(int eventId);

        bool RemoveById(int eventId);

        List<Event> GetEvents(string searchStringEventName, string searchStringCategoryName);
    }
}
