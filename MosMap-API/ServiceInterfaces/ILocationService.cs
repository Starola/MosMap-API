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
        public IEnumerable<Location> GetAllLocationsByCategoryId(int categoryId);
        public IEnumerable<Location> GetAllLocationsByCategoryIds(int[] categoryIds);
        public IEnumerable<Location> GetAllLocationsBySubCategoryId(int subcategoryId);
        public Location GetLocationById(int id);

        public Location CreateLocation(LocationForCreationDto locationDto);

        public Location UpdateLocation(int id, LocationForUpdateDto locationDto);
        public void DeleteLocation(Location location);
    }
}
