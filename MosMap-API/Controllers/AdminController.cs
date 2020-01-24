using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;
using MosMap_API.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace MosMap_API.Controllers
{
    [Authorize(Roles = "administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        private IMapper _mapper;

        public AdminController(IConfiguration config, IAdminService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("uncheckedlocations")]
        public async Task<IActionResult> GetAllLocationsForAdmin()
        {
            try
            {
                // returns all locations from database
                IEnumerable<LocationForAdminDto> locationsResult = await _service.GetAllUncheckedLocations();

                // Ok = status code 200
                return Ok(locationsResult);
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllLocations action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("alllocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                // returns all locations from database
                IEnumerable<LocationForAdminDto> locationsResult = await _service.GetAllLocations();

                // Ok = status code 200
                return Ok(locationsResult);
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllLocations action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("acceptlocations")]
        public async Task<IActionResult> AcceptLocation([FromBody] LocationForAcceptDto locationForAcceptDto)
        {
            try
            {
                _service.AcceptLocation(locationForAcceptDto);

                return Ok("Location was checked by admin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}