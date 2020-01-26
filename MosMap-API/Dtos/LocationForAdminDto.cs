using MosMap_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class LocationForAdminDto
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string Address { get; set; }

        public bool UserSuggestedLocation { get; set; }

        public bool ShowLocation { get; set; }

        public bool LocationChecked { get; set; }

        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<int> SubCategoryIds { get; set; }
    }
}
