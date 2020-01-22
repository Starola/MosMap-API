using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class LocationAsGeoJsonDto
    {
        public string Type { get; set; } = "Feature";
        public Geometry Geometry { get; set; } = new Geometry();
        public Property Properties { get; set; } = new Property();
        

    }

    public class Geometry
    {
        public string Type { get; set; } = "Point";
        public double[] Coordinates { get; set; } = new double[2];
    }

    public class Property
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool UserSuggestedLocation { get; set; }
        // public int CategoryId { get; set; }
        public List<int> SubCategoryIds { get; set; } = new List<int>();
    }
}
