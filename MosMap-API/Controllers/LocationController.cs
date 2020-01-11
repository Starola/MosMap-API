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
                IEnumerable<Location> locations = await _service.GetAllLocationsByCategoryId(categoryId);
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
                IEnumerable<Location> locations = await _service.GetAllLocationsByCategoryId(categoryId);
                // map location to locationdto
                IEnumerable<LocationDto> locationsDtos = _mapper.Map<IEnumerable<LocationDto>>(locations);

                // Convert locations to geojson format
                List<LocationAsGeoJsonDto> locationsResult = ConvertLocationDtoToGeoJson(locationsDtos);

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
                Location location = await _service.GetLocationById(id);

                if (location == null)
                {
                    // Location with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    LocationDto locationResult = _mapper.Map<LocationDto>(location);
                    return Ok(locationResult);
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
                Location location = await _service.GetLocationById(id);

                if (location == null)
                {
                    // Location with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    LocationDto locationDto = _mapper.Map<LocationDto>(location);
                    LocationAsGeoJsonDto locationResult = ConvertLocationDtoToGeoJson(locationDto);
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
        // implement return of Locations with multiple choosen categories (doesn't work yet)
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
        }


        // implement return of locations with choosen subcategory; also implement with multiple choosen subcategories!
        [HttpGet("subcategoryid/{subcategoryId}")]
        public async Task<IActionResult> GetAllLocationsBySubCategoryId(int subcategoryId)
        {
            IEnumerable<Location> locations = await _service.GetAllLocationsBySubCategoryId(subcategoryId);
            return Ok(locations);
        }

        // Create new Location --> only by admin/council (to be implemented!)
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

                LocationDto createdLocationDto = _mapper.Map<LocationDto>(createdLocation);

                return CreatedAtRoute("LocationById", new { id = createdLocationDto.Id }, createdLocationDto);
            }
            catch (Exception ex)
            {
                //Something went wrong inside CreateLocation action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Edit location --> only by admin/council (to be implemented!)
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

                Location locationEntity = await _service.GetLocationById(id);
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
        }

        // Delete location --> only by admin/council (to be implemented!)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                Location locationEntity = await _service.GetLocationById(id);
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

        #endregion


        /// <summary>
        /// Converts LocationDto to LocationAsGeoJsonDto
        /// </summary>
        /// <param name="locationDto"></param>
        /// <returns></returns>
        private LocationAsGeoJsonDto ConvertLocationDtoToGeoJson(LocationDto locationDto)
        {
            LocationAsGeoJsonDto loc = new LocationAsGeoJsonDto();
            loc.Geometry.Coordinates.SetValue(Convert.ToDouble(locationDto.Longitude, new CultureInfo("en-US")), 0);
            loc.Geometry.Coordinates.SetValue(Convert.ToDouble(locationDto.Latitude, new CultureInfo("en-US")), 1);
            loc.Properties.id = locationDto.Id;
            loc.Properties.Name = locationDto.LocationName;
            loc.Properties.Description = locationDto.LocationDescription;

            return loc;
        }

        /// <summary>
        /// Converts List of LocationDtos to List of LocationAsGeoJsonDtos
        /// </summary>
        /// <param name="locationDtos"></param>
        /// <returns></returns>
        private List<LocationAsGeoJsonDto> ConvertLocationDtoToGeoJson(IEnumerable<LocationDto> locationDtos)
        {
            List<LocationAsGeoJsonDto> locationsResult = new List<LocationAsGeoJsonDto>();
            locationDtos.ToList().ForEach(i =>
            {
                LocationAsGeoJsonDto loc = new LocationAsGeoJsonDto();
                loc.Geometry.Coordinates.SetValue(Convert.ToDouble(i.Longitude, new CultureInfo("en-US")), 0);
                loc.Geometry.Coordinates.SetValue(Convert.ToDouble(i.Latitude, new CultureInfo("en-US")), 1);
                loc.Properties.id = i.Id;
                loc.Properties.Name = i.LocationName;
                loc.Properties.Description = i.LocationDescription;
                locationsResult.Add(loc);
            });

            return locationsResult;
        }


    }
}