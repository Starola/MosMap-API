using MosMap_API.Dtos;
using MosMap_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.ServiceInterfaces
{
    public interface ILocationService
    {
        /// <summary>
        /// Returns list of locations with passed categoryid
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<IEnumerable<LocationDto>> GetAllLocationsByCategoryId(int categoryId);

        /// <summary>
        /// Returns location with passed locationid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<LocationDto> GetLocationById(int id);

        /// <summary>
        /// Returns location as GeoJson
        /// </summary>
        /// <param name="locationDto"></param>
        /// <returns></returns>
        Task<LocationAsGeoJsonDto> GetLocationsAsGeoJson(LocationDto locationDto);

        /// <summary>
        /// Returns list of locations as GeoJson
        /// </summary>
        /// <param name="locationDtos"></param>
        /// <returns></returns>
        Task<IEnumerable<LocationAsGeoJsonDto>> GetLocationsAsGeoJson(IEnumerable<LocationDto> locationDtos);


        Task<Location> CreateLocation(LocationForCreationDto locationDto);

        Task<Location> UpdateLocation(int id, LocationForUpdateDto locationDto);
        void DeleteLocation(LocationDto locationdto);



    }
}
