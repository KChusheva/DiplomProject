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
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = _context.Categories.ToList();
            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public List<Event> GetEventsByCategory(int categoryId)
        {
            return _context.Events.Where(x => x.CategoryId == categoryId).ToList();
        }

        public List<Reservation> GetReservationsByCategory(int categoryId)
        {
            return _context.Reservations.Where(x => x.CategoryId == categoryId).ToList();
        }
    }
}
