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
    public class LocationService : ILocationService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public LocationService(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        public Location CreateLocation(LocationForCreationDto locationDto)
        {
            Category category = _context.Categories
                .Where(i => i.Id.Equals(locationDto.CategoryId))
                .FirstOrDefault();

            /*SubCategory subCategory = _context.SubCategories
                .Where(i => i.Id.Equals(locationDto.SubCategoryId))
                .FirstOrDefault();*/

            Location location = new Location
            {
                LocationDescription = locationDto.LocationDescription,
                LocationName = locationDto.LocationName,
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude,
                Category = category
            };

            _context.Add(location);
            _context.SaveChanges();

            return location;
        }

        public void DeleteLocation(Location location)
        {
            _context.Remove(location);

            // delete all subcategorylocations with locationid of deleted location
            List<SubCategoryLocation> subCategoryLocations = _context.SubCategoryLocations
            .Where(j => j.Location.Id
            .Equals(location.Id))
            .ToList();

            if (subCategoryLocations.Count() != 0)
            {
                subCategoryLocations.ForEach(x => _context.Remove(x));
            }

            _context.SaveChanges();
        }

        public IEnumerable<Location> GetAllLocationsByCategoryId(int categoryId)
        {
            return _context.Locations
                .Where(i => i.Category.Id.Equals(categoryId))
                .ToList();
        }

        public IEnumerable<Location> GetAllLocationsByCategoryIds(int[] categoryIds)
        {
            List<Location> locationResult = new List<Location>();
            List<Location> locations = _context.Locations.ToList();
            foreach (int id in categoryIds.ToList())
            {
                locations.ForEach(i =>
                {
                    if (i.Category.Id.Equals(id))
                    {
                        locationResult.Add(i);
                    }
                });
            }
            return locationResult;
        }


        public IEnumerable<Location> GetAllLocationsBySubCategoryId(int subcategoryId)
        {
            List<SubCategoryLocation> subcategoryLocations = _context.SubCategoryLocations.Where(i => i.SubCategory.Id.Equals(subcategoryId)).ToList();
            List<Location> locations = _context.Locations.ToList();
            List<Location> locationsResult = new List<Location>();

            subcategoryLocations.ForEach(i =>
            {
                foreach (Location loc in locations)
                {
                    if (i.Location.Id.Equals(loc.Id))
                    {
                        locationsResult.Add(loc);
                    }
                }
            });

            return locationsResult;
        }

        public Location GetLocationById(int id)
        {
            return _context.Locations.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public Location UpdateLocation(int id, LocationForUpdateDto locationDto)
        {
            Category category = _context.Categories
                .Where(i => i.Id.Equals(locationDto.CategoryId))
                .FirstOrDefault();

            Location location = _context.Locations.Where(i => i.Id.Equals(id)).FirstOrDefault();

            location.LocationName = locationDto.LocationName;
            location.LocationDescription = locationDto.LocationDescription;
            location.Latitude = locationDto.Latitude;
            location.Longitude = locationDto.Longitude;
            location.Category = category;

            _context.Update(location);
            _context.SaveChanges();

            return location;
        }
    }
}
