﻿using AutoMapper;
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
    public class LocationService : ILocationService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IGeoJsonService _service;
        private IMapper _mapper;

        public LocationService(IConfiguration config, DataContext context, IGeoJsonService service, IMapper mapper)
        {
            _config = config;
            _context = context;
            _service = service;
            _mapper = mapper;
        }


        public async Task<IEnumerable<LocationDto>> GetAllLocationsByCategoryId(int categoryId)
        {
            List<Location> locations = await _context.Locations
                .Where(i => i.Category.Id.Equals(categoryId) && i.ShowLocation)
                .ToListAsync();
            List<LocationDto> locationsResult = _mapper.Map<List<LocationDto>>(locations);
            locationsResult.ForEach(i =>
            {
                //i.CategoryId = categoryId;
                i.SubCategoryIds = _context.SubCategoryLocations
                .Where(j => j.Location.Id.Equals(i.Id))
                .Select(j => j.SubCategory.Id)
                .ToList();
            });

            return locationsResult;
        }

        public async Task<LocationDto> GetLocationById(int id)
        {
            //return await _context.Locations.FirstOrDefaultAsync(i => i.Id.Equals(id));

            Location location = await _context.Locations.FirstOrDefaultAsync(i => i.Id.Equals(id));
            if(location == null)
            {
                return null;
            }
            if(!location.ShowLocation)
            {
                return null;
            }

            LocationDto locationDto = _mapper.Map<LocationDto>(location);
            //locationDto.CategoryId = location.Category.Id;
            locationDto.SubCategoryIds = await _context.SubCategoryLocations
                .Where(i => i.Location.Id.Equals(locationDto.Id))
                .Select(j => j.SubCategory.Id)
                .ToListAsync();

            return locationDto;
        }

        public async Task<LocationAsGeoJsonDto> GetLocationsAsGeoJson(LocationDto locationDto)
        {
            return await _service.ConvertLocationDtoToGeoJson(locationDto);
        }

        public async Task<IEnumerable<LocationAsGeoJsonDto>> GetLocationsAsGeoJson(IEnumerable<LocationDto> locationDtos)
        {
            return await _service.ConvertLocationDtoToGeoJson(locationDtos);
        }


        #region in progress!

        public async Task<Location> CreateLocation(LocationForCreationDto locationDto)
        {
            // Create new location with data from locationdto
            Location location = new Location
            {
                LocationDescription = locationDto.LocationDescription,
                LocationName = locationDto.LocationName,
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude,
                Address = locationDto.Address,
                Category = await _context.Categories.FirstOrDefaultAsync(i => i.Id.Equals(locationDto.CategoryId)),
                UserSuggestedLocation = true,
                LocationChecked = false,
                ShowLocation = false,
            };

            // check, if user is admin, council
            if (/*user == admin || user == council*/ true)
            {
                location.UserSuggestedLocation = false;
                location.LocationChecked = true;
                location.ShowLocation = true;
            }

            await _context.AddAsync(location);
            await _context.SaveChangesAsync();

            // connect location with subcategories
            foreach (int id in locationDto.SubCategoryIds)
            {
                SubCategoryLocation subCategoryLocation = new SubCategoryLocation
                {
                    Location = location,
                    SubCategory = _context.SubCategories.FirstOrDefault(i => i.Id.Equals(id))
                };
                _context.SubCategoryLocations.Add(subCategoryLocation);
                _context.SaveChanges();
            }

            return location;
        }
        #endregion


        #region further methods (not used)
        // Methode nochmal anpassen!
        /*public async Task<IEnumerable<Location>> GetAllLocationsByCategoryIds(int[] categoryIds)
        {
            List<Location> locationResult = new List<Location>();
            List<Location> locations = await _context.Locations.ToListAsync();
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
        }*/

        /*// Methode nochmal anpassen!
        public async Task<IEnumerable<Location>> GetAllLocationsBySubCategoryId(int subcategoryId)
        {
            List<SubCategoryLocation> subcategoryLocations = await _context.SubCategoryLocations
                .Where(i => i.SubCategory.Id.Equals(subcategoryId))
                .ToListAsync();
            List<Location> locations = await _context.Locations.ToListAsync();
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
        }*/

        /*
        // Methode nochmal anpassen!
        public async Task<Location> UpdateLocation(int id, LocationForUpdateDto locationDto)
        {
            Category category = await _context.Categories
                .FirstOrDefaultAsync(i => i.Id.Equals(locationDto.CategoryId));

            Location location = await _context.Locations.FirstOrDefaultAsync(i => i.Id.Equals(id));

            location.LocationName = locationDto.LocationName;
            location.LocationDescription = locationDto.LocationDescription;
            location.Latitude = locationDto.Latitude;
            location.Longitude = locationDto.Longitude;
            location.Category = category;

            _context.Update(location);
            await _context.SaveChangesAsync();

            return location;
        }*/


        /*
        // Methode nochmal anpassen!
        public void DeleteLocation(LocationDto location)
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
        }*/
        #endregion



    }
}
