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


        [HttpGet("{categoryid}")]
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
    }
}