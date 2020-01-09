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
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _service;
        private IMapper _mapper;

        public SubCategoryController(IConfiguration config, ISubCategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("categoryid/{categoryid}")]
        public IActionResult GetAllSubCategories(int categoryId)
        {
            try
            {
                // returns all subcategories from database
                IEnumerable<SubCategory> subcategories = _service.GetAllSubCategories(categoryId);

                if(subcategories.Count() == 0)
                {
                    // no subcategories were found with categoryid
                    return NotFound();
                }
                else
                {
                    // map subcategory to categorydto
                    IEnumerable<SubCategoryDto> subcategoriesResult = _mapper.Map<IEnumerable<SubCategoryDto>>(subcategories);
                    // Ok = status code 200
                    return Ok(subcategoriesResult);
                }
                
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllCategories action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "SubCategoryById")]
        public IActionResult GetSubCategoryById(int id)
        {
            try
            {
                SubCategory subcategory = _service.GetSubCategoryById(id);

                if (subcategory == null)
                {
                    // subcategory with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    SubCategoryWithDetailsDto subcategoryResult = _mapper.Map<SubCategoryWithDetailsDto>(subcategory);
                    return Ok(subcategoryResult);
                }
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetCategoryById action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateNewSubCategory([FromBody]SubCategoryForCreationDto subcategory)
        {
            try
            {
                if (subcategory == null)
                {
                    // subcategory object sent from client is null
                    return BadRequest("Subcategory object is null");
                }

                if (!ModelState.IsValid)
                {
                    // Invalid subcategory object sent from client
                    return BadRequest("Invalid model object");
                }

                SubCategory createdSubCategory = _service.CreateSubCategory(subcategory);

                SubCategoryDto createdSubCategoryDto = _mapper.Map<SubCategoryDto>(createdSubCategory);

                return CreatedAtRoute("SubCategoryById", new { id = createdSubCategoryDto.Id }, createdSubCategoryDto);
            }
            catch (Exception ex)
            {
                //Something went wrong inside CreateCategory action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditSubCategory(int id, [FromBody] SubCategoryForUpdateDto subcategory)
        {
            try
            {
                if (subcategory == null)
                {
                    // subcategory object sent from client is null
                    return BadRequest("Category object is null");
                }

                if (!ModelState.IsValid)
                {
                    // Invalid subcategory object sent from client 
                    return BadRequest("Invalid model object");
                }

                SubCategory subcategoryEntity = _service.GetSubCategoryById(id);
                if (subcategoryEntity == null)
                {
                    // subcategory with id hasn't been found in db
                    return NotFound();
                }

                SubCategory updatedSubCategory = _service.UpdateSubCategory(id, subcategory);

                SubCategoryWithDetailsDto updatedSubCategoryDto = _mapper.Map<SubCategoryWithDetailsDto>(subcategoryEntity);

                return CreatedAtRoute("SubCategoryById", new { id = id }, updatedSubCategoryDto);
            }
            catch (Exception ex)
            {
                // Something went wrong inside UpdateCategory action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubCategory(int id)
        {
            try
            {
                SubCategory subcategoryEntity = _service.GetSubCategoryById(id);
                if (subcategoryEntity == null)
                {
                    // subcategory with id hasn't been found in db
                    return NotFound();
                }
                _service.DeleteSubCategory(subcategoryEntity);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Something went wrong inside DeleteSubCategory action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}