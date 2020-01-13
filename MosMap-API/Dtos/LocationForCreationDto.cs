using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class LocationForCreationDto
    {
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public int CategoryId {get; set;}
        // public List<int> SubCategoryIds {get; set;}
        // User?
    }
}
