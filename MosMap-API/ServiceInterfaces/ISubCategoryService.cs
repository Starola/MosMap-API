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
        IEnumerable<SubCategory> GetAllSubCategories(int categoryId);
        SubCategory GetSubCategoryById(int id);
        SubCategory CreateSubCategory(SubCategoryForCreationDto subCategoryDto);
        SubCategory UpdateSubCategory(int id, SubCategoryForUpdateDto subCategoryDto);
        void DeleteSubCategory(SubCategory subCategory);
    }
}
