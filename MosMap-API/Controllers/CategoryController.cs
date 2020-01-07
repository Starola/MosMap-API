using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MosMap_API.Dtos;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;

namespace MosMap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private IMapper _mapper;

        public CategoryController(IConfiguration config, ICategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                // returns all categories from database
                IEnumerable<Category> categories = _service.GetAllCategories();
                // map category to categorydto
                IEnumerable<CategoryDto> categoriesResult = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                // Ok = status code 200
                return Ok(categoriesResult);
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
                CategoryDto categoryResult = _mapper.Map<CategoryDto>(category);

                if (category == null)
                {
                    // Category with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    return Ok(categoryResult);
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