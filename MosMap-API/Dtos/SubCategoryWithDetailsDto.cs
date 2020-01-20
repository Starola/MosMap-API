using MosMap_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class SubCategoryWithDetailsDto
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }

        public CategoryDto Category { get; set; }
    }
}
