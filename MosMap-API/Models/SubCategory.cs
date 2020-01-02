using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<SubCategoryLocation> SubCategoryLocations { get; set; }
    }
}
