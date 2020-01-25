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
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace MosMap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _service;
        private IMapper _mapper;

        public LocationController(IConfiguration config, ILocationService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns List of Locations (LocationDtos) with passed categoryid
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("categoryid/{categoryId}")]
        public async Task<IActionResult> GetAllLocationsByCategoryId(int categoryId)
        {
            try
            {
                // returns all locations from database
                IEnumerable<LocationDto> locationsResult = await _service.GetAllLocationsByCategoryId(categoryId);

                // Ok = status code 200
                return Ok(locationsResult);
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllLocations action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns List of Locations (GeoJsonLocations) with passed categoryid
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("geojson/categoryid/{categoryId}")]
        public async Task<IActionResult> GetAllGeoJsonLocationsByCategoryId(int categoryId)
        {
            try
            {
                // returns all locations from database
                IEnumerable<LocationDto> locationsDtos = await _service.GetAllLocationsByCategoryId(categoryId);

                // Convert locations to geojson format
                IEnumerable <LocationAsGeoJsonDto> locationsResult = await _service.GetLocationsAsGeoJson(locationsDtos);

                // Ok = status code 200
                return Ok(locationsResult);
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllLocations action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns Location (LocationDto) with passed locationid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "LocationById")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            try
            {
                LocationDto location = await _service.GetLocationById(id);

                if (location == null)
                {
                    // Location with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    return Ok(location);
                }
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetLocationById action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns Location (GeoJsonLocation) with passed locationid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("geojson/{id}", Name = "GeoJsonLocationById")]
        public async Task<IActionResult> GetGeoJsonLocationById(int id)
        {
            try
            {
                LocationDto location = await _service.GetLocationById(id);

                if (location == null)
                {
                    // Location with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    LocationAsGeoJsonDto locationResult = await _service.GetLocationsAsGeoJson(location);
                    return Ok(locationResult);
                }
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetLocationById action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody]LocationForCreationDto location)
        {
            try
            {
                if (location == null)
                {
                    // location object sent from client is null
                    return BadRequest("Location object is null");
                }

                if (!ModelState.IsValid)
                {
                    // Invalid location object sent from client
                    return BadRequest("Invalid model object");
                }

                Location createdLocation = await _service.CreateLocation(location);

                return Ok("Location was created!");
            }
            catch (Exception ex)
            {
                //Something went wrong inside CreateLocation action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditLocation(int id, [FromBody] LocationForUpdateDto location)
        {
            try
            {
                if (location == null)
                {
                    // location object sent from client is null
                    return BadRequest("LOcation object is null");
                }

                if (!ModelState.IsValid)
                {
                    // Invalid location object sent from client 
                    return BadRequest("Invalid model object");
                }

                LocationDto locationEntity = await _service.GetLocationById(id);
                if (locationEntity == null)
                {
                    // location with id hasn't been found in db
                    return NotFound();
                }

                Location updatedLocation = await _service.UpdateLocation(id, location);

                return Ok("Location was edited!");
            }
            catch (Exception ex)
            {
                // Something went wrong inside UpdateLocation action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                LocationDto locationEntity = await _service.GetLocationById(id);
                if (locationEntity == null)
                {
                    // loaction with id hasn't been found in db
                    return NotFound();
                }
                _service.DeleteLocation(locationEntity);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Something went wrong inside DeleteLocation action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}