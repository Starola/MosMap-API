using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Models
{
    public class SubCategoryLocation
    {
        public int Id { get; set; }
        public SubCategory SubCategory { get; set; }
        public Location Location { get; set; }
    }
}
