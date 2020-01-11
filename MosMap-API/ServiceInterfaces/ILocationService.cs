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
        Task <IEnumerable<Location>> GetAllLocationsByCategoryId(int categoryId);
        Task <IEnumerable<Location>> GetAllLocationsByCategoryIds(int[] categoryIds);
        Task <IEnumerable<Location>> GetAllLocationsBySubCategoryId(int subcategoryId);
        Task<Location> GetLocationById(int id);

        Task<Location> CreateLocation(LocationForCreationDto locationDto);

        Task<Location> UpdateLocation(int id, LocationForUpdateDto locationDto);
        void DeleteLocation(Location location);

        Task<Location> CreateLocationByUser(LocationForCreationDto locationDto);
    }
}
