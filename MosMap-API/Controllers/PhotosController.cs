
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MosMap_API.Helpers;
using Microsoft.Extensions.Options;
using MosMap_API.Dtos;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;

namespace MosMap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PhotosController : ControllerBase
    {
        private IPhotosService _photosService;
        
        public PhotosController(IPhotosService photosService)
        {
            _photosService = photosService;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await this._photosService.GetPhoto(id);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForLocation(int locationId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            var photo =await this._photosService.AddPhotoForLocation(locationId, photoForCreationDto);

            return CreatedAtRoute("GetPhoto", new {locationId = locationId, id = photo.Id}, photo);


        }
        
    }
}