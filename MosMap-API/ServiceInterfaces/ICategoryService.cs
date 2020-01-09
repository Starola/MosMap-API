using MosMap_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.ServiceInterfaces
{
    public interface ICategoryService
    {
        public IEnumerable<Category> GetAllCategories();
        public Category GetCategoryById(int id);
        public void CreateCategory(Category category);
        public void UpdateCategory(Category category);
        public void DeleteCategory(Category category);
    }
}
