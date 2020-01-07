using MosMap_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.ServiceInterfaces
{
    public interface ISubCategoryService
    {
        public IEnumerable<SubCategory> GetAllSubCategories(int categoryId);
    }
}
