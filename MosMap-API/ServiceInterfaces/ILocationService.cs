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
        IEnumerable<Location> GetAllLocationsByCategoryId(int categoryId);
        IEnumerable<Location> GetAllLocationsByCategoryIds(int[] categoryIds);
        IEnumerable<Location> GetAllLocationsBySubCategoryId(int subcategoryId);
        Location GetLocationById(int id);

        Location CreateLocation(LocationForCreationDto locationDto);

        Location UpdateLocation(int id, LocationForUpdateDto locationDto);
        void DeleteLocation(Location location);

        Location CreateLocationByUser(LocationForCreationDto locationDto);
    }
}
