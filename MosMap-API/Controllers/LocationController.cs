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
                    //LocationDto locationResult = _mapper.Map<LocationDto>(location);
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


        #region in progress
        // Create new Location (to be implemented!)
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

                //LocationDto createdLocationDto = await _service.GetLocationById(createdLocation.Id);

                //return CreatedAtRoute("LocationById", new { id = createdLocationDto.Id }, createdLocationDto);
                return Ok("Location was created!");
            }
            catch (Exception ex)
            {
                //Something went wrong inside CreateLocation action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region further methods (not used)
        /*// implement return of Locations with multiple choosen categories (doesn't work yet)
        [HttpGet("categoryids/{categoryIds}")]
        public async Task<IActionResult> GetAllLocationsByCategoryIds([FromQuery] int[] categoryIds)
        {
            try
            {
                // returns all locations from database
                IEnumerable<Location> locations = await _service.GetAllLocationsByCategoryIds(categoryIds);
                // map location to locationdto
                IEnumerable<LocationDto> locationsResult = _mapper.Map<IEnumerable<LocationDto>>(locations);
                // Ok = status code 200
                return Ok(locationsResult);
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllLocations action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }*/


        /*// implement return of locations with choosen subcategory; also implement with multiple choosen subcategories!
        [HttpGet("subcategoryid/{subcategoryId}")]
        public async Task<IActionResult> GetAllLocationsBySubCategoryId(int subcategoryId)
        {
            IEnumerable<Location> locations = await _service.GetAllLocationsBySubCategoryId(subcategoryId);
            return Ok(locations);
        }*/



        // Edit location --> only by admin/council (to be implemented!)
        /*[HttpPut("{id}")]
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

                LocationDto updatedLocationDto = _mapper.Map<LocationDto>(locationEntity);

                return CreatedAtRoute("LocationById", new { id = id }, updatedLocationDto);
            }
            catch (Exception ex)
            {
                // Something went wrong inside UpdateLocation action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }*/

        // Delete location --> only by admin/council (to be implemented!)
        /*[HttpDelete("{id}")]
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
        }*/

        #endregion






    }
}