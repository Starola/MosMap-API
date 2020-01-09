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

        [HttpGet("{id}", Name = "CategoryById")]
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
                    CategoryDto categoryResult = _mapper.Map<CategoryDto>(category);
                    return Ok(categoryResult);
                }
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetCategoryById action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult CreateNewCategory([FromBody]CategoryForCreationDto category)
        {
            try
            {
                if (category == null)
                {
                    // Category object sent from client is null
                    return BadRequest("Category object is null");
                }

                if (!ModelState.IsValid)
                {
                    // Invalid category object sent from client
                    return BadRequest("Invalid model object");
                }

                Category categoryEntity = _mapper.Map<Category>(category);
                _service.CreateCategory(categoryEntity);

                CategoryDto createdCategory = _mapper.Map<CategoryDto>(categoryEntity);

                return CreatedAtRoute("CategoryById", new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                //Something went wrong inside CreateCategory action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditCategory(int id, [FromBody] CategoryForUpdateDto category)
        {
            try
            {
                if (category == null)
                {
                    // Category object sent from client is null
                    return BadRequest("Category object is null");
                }

                if (!ModelState.IsValid)
                {
                    //n Invalid category object sent from client 
                    return BadRequest("Invalid model object");
                }

                Category categoryEntity = _service.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    // Category with id hasn't been found in db
                    return NotFound();
                }

                _mapper.Map(category, categoryEntity);

                _service.UpdateCategory(categoryEntity);

                //return NoContent();
                CategoryDto updatedCategory = _mapper.Map<CategoryDto>(categoryEntity);

                return CreatedAtRoute("CategoryById", new { id = id }, updatedCategory);
            }
            catch (Exception ex)
            {
                // Something went wrong inside UpdateCategory action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                Category categoryEntity = _service.GetCategoryById(id);
                if (categoryEntity == null)
                {
                    // Category with id hasn't been found in db
                    return NotFound();
                }
                _service.DeleteCategory(categoryEntity);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Something went wrong inside DeleteCategory action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}