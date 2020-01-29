using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MosMap_API.Models;

namespace MosMap_API.Dtos
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string Address { get; set; }

        public bool UserSuggestedLocation { get; set; }

        public int CategoryId { get; set; }
        public List<int> SubCategoryIds { get; set; }
        
        public string PhotoUrl { get; set; }

        public ICollection<PhotoForDetailDto> Photos { get; set; }

    }
}
