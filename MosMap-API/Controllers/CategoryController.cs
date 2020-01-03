using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;

namespace MosMap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(IConfiguration config, ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                // returns all categories from database
                IEnumerable<Category> categories = _service.GetAllCategories();
                // Ok = status code 200
                return Ok(categories);
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllCategories action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                Category category = _service.GetCategoryById(id);

                if (category == null)
                {
                    // Category with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    return Ok(category);
                }
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetCategoryById action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}