using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MosMap_API.Data;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public CategoryService(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories
                .Include(sub => sub.SubCategories)
                .Include(loc => loc.Locations)
                .ToList();
            
            //return _context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories
                .Where(i => i.Id.Equals(id))
                .Include(sub => sub.SubCategories)
                .Include(loc => loc.Locations)
                .FirstOrDefault();
            
            //return _context.Categories.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }
    }
}
