using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string LocationDescription { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        
        public string Address { get; set; }

        // true: User suggested location; false: default location
        public bool UserSuggestedLocation { get; set; }
        /* true: User suggested location was checked and permitted by admin and will be shown in map
        *       if admin didn't permit location, location will be deleted
        * false: User suggested location was not checked by admin and will not be shown in map
        */
        public bool ShowLocation { get; set; }
        
        public bool LocationChecked { get; set; }

        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<SubCategoryLocation> SubCategoryLocations { get; set; }
    }
}
