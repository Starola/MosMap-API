using MosMap_API.Dtos;
using MosMap_API.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Services
{
    public class GeoJsonService : IGeoJsonService
    {
        public async Task<LocationAsGeoJsonDto> ConvertLocationDtoToGeoJson(LocationDto locationDto)
        {
            LocationAsGeoJsonDto loc = new LocationAsGeoJsonDto();
            loc.Geometry.Coordinates.SetValue(Convert.ToDouble(locationDto.Longitude, new CultureInfo("en-US")), 0);
            loc.Geometry.Coordinates.SetValue(Convert.ToDouble(locationDto.Latitude, new CultureInfo("en-US")), 1);
            loc.Properties.id = locationDto.Id;
            loc.Properties.Name = locationDto.LocationName;
            loc.Properties.Description = locationDto.LocationDescription;
            loc.Properties.CategoryId = locationDto.CategoryId;
            loc.Properties.SubCategoryIds = locationDto.SubCategoryIds;
            loc.Properties.Address = locationDto.Address;
            loc.Properties.UserSuggestedLocation = locationDto.UserSuggestedLocation;

            return loc;
        }

        public async Task<List<LocationAsGeoJsonDto>> ConvertLocationDtoToGeoJson(IEnumerable<LocationByCategoryDto> locationDtos)
        {
            List<LocationAsGeoJsonDto> locationsResult = new List<LocationAsGeoJsonDto>();
            locationDtos.ToList().ForEach(i =>
            {
                LocationAsGeoJsonDto loc = new LocationAsGeoJsonDto();
                loc.Geometry.Coordinates.SetValue(Convert.ToDouble(i.Longitude, new CultureInfo("en-US")), 0);
                loc.Geometry.Coordinates.SetValue(Convert.ToDouble(i.Latitude, new CultureInfo("en-US")), 1);
                loc.Properties.id = i.Id;
                loc.Properties.Name = i.LocationName;
                loc.Properties.Description = i.LocationDescription;
                loc.Properties.CategoryId = i.CategoryId;
                loc.Properties.SubCategoryIds = i.SubCategoryIds;
                loc.Properties.Address = i.Address;
                loc.Properties.UserSuggestedLocation = i.UserSuggestedLocation;
                locationsResult.Add(loc);
            });

            return locationsResult;
        }
    }
}
