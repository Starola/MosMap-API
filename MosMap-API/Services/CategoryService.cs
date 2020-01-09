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
            /*return _context.Categories
                .Include(sub => sub.SubCategories)
                .Include(loc => loc.Locations)
                .ToList();*/

            return _context.Categories.ToList();
        }

        public Category GetCategoryById(int id)
        {
            /*return _context.Categories
                .Where(i => i.Id.Equals(id))
                .Include(sub => sub.SubCategories)
                .Include(loc => loc.Locations)
                .FirstOrDefault();*/

            return _context.Categories.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public void CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            _context.Remove(category);
            //_context.SaveChanges();

            // delete all subcategories with categoryid of deleted category
            List<SubCategory> subcategories = _context.SubCategories
                .Where(i => i.Category.Id
                .Equals(category.Id))
                .ToList();

            if (subcategories.Count != 0)
            {
                subcategories.ForEach(i =>
                {
                    _context.Remove(i);

                    // delete all subcategorylocations with subcategoryid of deleted subcategory
                    List<SubCategoryLocation> subCategoryLocations = _context.SubCategoryLocations
                    .Where(j => j.SubCategory.Id
                    .Equals(i.Id))
                    .ToList();

                    if (subCategoryLocations.Count() != 0)
                    {
                        subCategoryLocations.ForEach(x => _context.Remove(x));
                    }

                });
            }

            // delete all locations with categoryid of deleted category
            List<Location> locations = _context.Locations
                .Where(i => i.Category.Id
                .Equals(category.Id))
                .ToList();

            if (locations.Count != 0)
            {
                locations.ForEach(i =>
                {
                    _context.Remove(i);

                    // delete all subcategorylocations with locationid of deleted location
                    List<SubCategoryLocation> subCategoryLocations = _context.SubCategoryLocations
                    .Where(j => j.Location.Id
                    .Equals(i.Id))
                    .ToList();

                    if (subCategoryLocations.Count() != 0)
                    {
                        subCategoryLocations.ForEach(x => _context.Remove(x));
                    }
                });
            }
            _context.SaveChanges();
        }
    }
}
