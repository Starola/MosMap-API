using MosMap_API.Dtos;
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
        public SubCategory GetSubCategoryById(int id);
        public SubCategory CreateSubCategory(SubCategoryForCreationDto subCategoryDto);
        public SubCategory UpdateSubCategory(int id, SubCategoryForUpdateDto subCategoryDto);
        public void DeleteSubCategory(SubCategory subCategory);
    }
}
