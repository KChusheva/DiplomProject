using KristaRecords.Infrastructure.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KristaRecords.Core.Contracts
{
    public interface ICategoryService
    {
        List<Category> GetCategories();
        Category GetCategoryById(int categoryId);
        List<Reservation> GetReservationsByCategory(int categoryId);
        List<Event> GetEventsByCategory(int categoryId);
    }
}
