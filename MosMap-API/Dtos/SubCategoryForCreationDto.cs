using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class SubCategoryForCreationDto
    {
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
        public int CategoryId { get; set; }
    }
}
