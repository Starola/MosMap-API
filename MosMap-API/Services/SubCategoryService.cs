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
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public SubCategoryService(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }
        public IEnumerable<SubCategory> GetAllSubCategories(int categoryId)
        {
            return _context.SubCategories.Where(i => i.Category.Id.Equals(categoryId));
        }
    }
}

