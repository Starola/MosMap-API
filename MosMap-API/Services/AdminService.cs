using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class AdminService : IAdminService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private IMapper _mapper;

        public AdminService(IConfiguration config, DataContext context, IMapper mapper)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
        }

        public void AcceptLocation(LocationForAcceptDto locationForAcceptDto)
        {
            Location location = _context.Locations.FirstOrDefault(i => i.Id.Equals(locationForAcceptDto.LocationId));
            location.LocationChecked = true;

            if (locationForAcceptDto.Accepted)
            {
                location.ShowLocation = true;
            }

            _context.SaveChanges();
        }

        public async Task<IEnumerable<LocationForAdminDto>> GetAllLocations()
        {
            List<Location> locations = await _context.Locations
                .Include(i => i.Category)
                .Include(i => i.User)
                .ToListAsync();


            List<LocationForAdminDto> locationsResult = _mapper.Map<List<LocationForAdminDto>>(locations);
            locationsResult.ForEach(i =>
            {
                i.SubCategoryIds = _context.SubCategoryLocations
                .Where(j => j.Location.Id.Equals(i.Id))
                .Select(j => j.SubCategory.Id)
                .ToList();

                i.CategoryName = _context.Categories
                .Where(j => j.Id.Equals(i.CategoryId))
                .Select(j => j.CategoryName)
                .FirstOrDefault();
            });

            return locationsResult;
        }

        public async Task<IEnumerable<LocationForAdminDto>> GetAllUncheckedLocations()
        {
            List<Location> locations = await _context.Locations
                .Where(i => !i.LocationChecked)
                .Include(i => i.Category)
                .Include(i => i.User)
                .ToListAsync();

            List<LocationForAdminDto> locationsResult = _mapper.Map<List<LocationForAdminDto>>(locations);
            locationsResult.ForEach(i =>
            {
                i.SubCategoryIds = _context.SubCategoryLocations
                .Where(j => j.Location.Id.Equals(i.Id))
                .Select(j => j.SubCategory.Id)
                .ToList();

                i.CategoryName = _context.Categories
                .Where(j => j.Id.Equals(i.CategoryId))
                .Select(j => j.CategoryName)
                .FirstOrDefault();
            });

            return locationsResult;
        }
    }
}
