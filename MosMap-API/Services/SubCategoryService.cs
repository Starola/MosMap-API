﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MosMap_API.Data;
using MosMap_API.Dtos;
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

        public async Task<SubCategory> CreateSubCategory(SubCategoryForCreationDto subCategoryDto)
        {
            Category category = _context.Categories
                .Where(i => i.Id.Equals(subCategoryDto.CategoryId))
                .FirstOrDefault();

            SubCategory subCategory = new SubCategory
            {
                SubCategoryName = subCategoryDto.SubCategoryName,
                SubCategoryDescription = subCategoryDto.SubCategoryDescription,
                Category = category
            };

            await _context.AddAsync(subCategory);
            await _context.SaveChangesAsync();

            return subCategory;
        }

        public void DeleteSubCategory(SubCategory subCategory)
        {
            _context.Remove(subCategory);

            // delete all subcategorylocations with subcategoryid of deleted subcategory
            List<SubCategoryLocation> subCategoryLocations = _context.SubCategoryLocations
            .Where(j => j.SubCategory.Id
            .Equals(subCategory.Id))
            .ToList();

            if (subCategoryLocations.Count() != 0)
            {
                subCategoryLocations.ForEach(x => _context.Remove(x));
            }

            _context.SaveChanges();
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategories(int categoryId)
        {
            return await _context.SubCategories
                .Where(i => i.Category.Id.Equals(categoryId))
                .Include(i => i.Category)
                .ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryById(int id)
        {
            return await _context.SubCategories
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.Id.Equals(id));

        }

        public async Task<SubCategory> UpdateSubCategory(int id, SubCategoryForUpdateDto subCategoryDto)
        {
            Category category = _context.Categories
                .Where(i => i.Id.Equals(subCategoryDto.CategoryId))
                .FirstOrDefault();

            SubCategory subCategory = _context.SubCategories.Where(i => i.Id.Equals(id)).FirstOrDefault();

            subCategory.SubCategoryName = subCategoryDto.SubCategoryName;
            subCategory.SubCategoryDescription = subCategoryDto.SubCategoryDescription;
            subCategory.Category = category;

            _context.Update(subCategory);
            await _context.SaveChangesAsync();

            return subCategory;
        }
    }
}

