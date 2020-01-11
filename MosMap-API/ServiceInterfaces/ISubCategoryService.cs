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
        Task<IEnumerable<SubCategory>> GetAllSubCategories(int categoryId);
        Task <SubCategory> GetSubCategoryById(int id);
        Task <SubCategory> CreateSubCategory(SubCategoryForCreationDto subCategoryDto);
        Task<SubCategory> UpdateSubCategory(int id, SubCategoryForUpdateDto subCategoryDto);
        void DeleteSubCategory(SubCategory subCategory);
    }
}
