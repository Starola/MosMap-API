using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MosMap_API.Data;
using MosMap_API.Dtos;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Services
{
    public class LocationService : ILocationService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IGeoJsonService _gjservice;
        private readonly IUserService _uservice;
        private IMapper _mapper;

        public LocationService(IConfiguration config, DataContext context, IGeoJsonService gjservice, IUserService uservice, IMapper mapper)
        {
            _config = config;
            _context = context;
            _gjservice = gjservice;
            _uservice = uservice;
            _mapper = mapper;
        }


        public async Task<IEnumerable<LocationDto>> GetAllLocationsByCategoryId(int categoryId)
        {
            List<Location> locations = await _context.Locations
                .Where(i => i.Category.Id.Equals(categoryId) && i.ShowLocation)
                .Include(i => i.Category)
                .ToListAsync();
            List<LocationDto> locationsResult = _mapper.Map<List<LocationDto>>(locations);
            locationsResult.ForEach(i =>
            {
                i.SubCategoryIds = _context.SubCategoryLocations
                .Where(j => j.Location.Id.Equals(i.Id))
                .Select(j => j.SubCategory.Id)
                .ToList();
            });

            return locationsResult;
        }

        public async Task<LocationDto> GetLocationById(int id)
        {
            Location location = await _context.Locations.Include(i => i.Category).Include(i => i.Photos).FirstOrDefaultAsync(i => i.Id.Equals(id));
            if(location == null)
            {
                return null;
            }
            if(!location.ShowLocation && !_uservice.IsAdmin())
            {
                return null;
            }

            LocationDto locationDto = _mapper.Map<LocationDto>(location);
            locationDto.SubCategoryIds = await _context.SubCategoryLocations
                .Where(i => i.Location.Id.Equals(locationDto.Id))
                .Select(j => j.SubCategory.Id)
                .ToListAsync();

            return locationDto;
        }

        public async Task<LocationAsGeoJsonDto> GetLocationsAsGeoJson(LocationDto locationDto)
        {
            return await _gjservice.ConvertLocationDtoToGeoJson(locationDto);
        }

        public async Task<IEnumerable<LocationAsGeoJsonDto>> GetLocationsAsGeoJson(IEnumerable<LocationDto> locationDtos)
        {
            return await _gjservice.ConvertLocationDtoToGeoJson(locationDtos);
        }



        public async Task<Location> CreateLocation(LocationForCreationDto locationDto)
        {
            string latitudedto = ReplaceCommaInCoordinate(locationDto.Latitude);

            string longitudedto = ReplaceCommaInCoordinate(locationDto.Longitude);

            if(!CheckCoordinateLimit(latitudedto, longitudedto))
            {
                throw new Exception(message: "Die Koordinaten liegen außerhalb des Bereiches von Mosbach");
            }

            // Create new location with data from locationdto
            Location location = new Location
            {
                LocationDescription = locationDto.LocationDescription,
                LocationName = locationDto.LocationName,
                Latitude = latitudedto,
                Longitude = longitudedto,
                Address = locationDto.Address,
                Category = await _context.Categories.FirstOrDefaultAsync(i => i.Id.Equals(locationDto.CategoryId)),
                UserSuggestedLocation = true,
                LocationChecked = false,
                ShowLocation = false,
                User = await _context.ApplicationUser.FirstOrDefaultAsync(i => i.Id.Equals(_uservice.GetUserId()))
            };

            // check, if user is admin, council
            if (_uservice.IsAdmin() || _uservice.IsCouncil())
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

        private string ReplaceCommaInCoordinate(string coordinate)
        {
            if (coordinate.Contains(','))
            {
                coordinate = coordinate.Replace(',', '.');
            }
            return coordinate;
        }

        private bool CheckCoordinateLimit(string latitudedto, string longitudedto)
        {
            // coordinates limit:   49.472380/9.047680    49.472380/9.209718
            //                      49.316411/9.047680    49.316411/9.209718
            double latit = Convert.ToDouble(latitudedto, new CultureInfo("en-US"));
            double longit = Convert.ToDouble(longitudedto, new CultureInfo("en-US"));

            if (latit > 49.472380 || latit < 49.316411 || longit < 9.047680 || longit > 9.209718)
            {
                return false;
            }
            return true;
        }


        public async Task<Location> UpdateLocation(int id, LocationForUpdateDto locationDto)
        {
            Category category = await _context.Categories
                .FirstOrDefaultAsync(i => i.Id.Equals(locationDto.CategoryId));

            string latitudedto = ReplaceCommaInCoordinate(locationDto.Latitude);

            string longitudedto = ReplaceCommaInCoordinate(locationDto.Longitude);

            if (!CheckCoordinateLimit(latitudedto, longitudedto))
            {
                throw new Exception(message: "Die Koordinaten liegen außerhalb des Bereiches von Mosbach");
            }


            Location location = await _context.Locations.FirstOrDefaultAsync(i => i.Id.Equals(id));

            location.LocationName = locationDto.LocationName;
            location.LocationDescription = locationDto.LocationDescription;
            location.Latitude = latitudedto;
            location.Longitude = longitudedto;
            location.Address = locationDto.Address;
            location.Category = category;

            _context.Update(location);
            await _context.SaveChangesAsync();

            // delete all subcategorylocations with locationid of edited location
            List<SubCategoryLocation> subCategoryLocations = _context.SubCategoryLocations
            .Where(i => i.Location.Id
            .Equals(location.Id))
            .ToList();

            if (subCategoryLocations.Count() != 0)
            {
                subCategoryLocations.ForEach(x => _context.Remove(x));
            }

            _context.SaveChanges();

            // connect location with subcategories
            foreach (int subCatId in locationDto.SubCategoryIds)
            {
                SubCategoryLocation subCategoryLocation = new SubCategoryLocation
                {
                    Location = location,
                    SubCategory = _context.SubCategories.FirstOrDefault(i => i.Id.Equals(subCatId))
                };
                _context.SubCategoryLocations.Add(subCategoryLocation);
                _context.SaveChanges();
            }

            return location;
        }

        
        public void DeleteLocation(LocationDto locationdto)
        {
            Location location = _context.Locations.FirstOrDefault(i => i.Id.Equals(locationdto.Id));
            _context.Remove(location);

            // delete all subcategorylocations with locationid of deleted location
            List<SubCategoryLocation> subCategoryLocations = _context.SubCategoryLocations
            .Where(i => i.Location.Id
            .Equals(locationdto.Id))
            .ToList();

            if (subCategoryLocations.Count() != 0)
            {
                subCategoryLocations.ForEach(x => _context.Remove(x));
            }

            _context.SaveChanges();
        }

    }
}
