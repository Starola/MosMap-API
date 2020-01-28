using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MosMap_API.Data;
using MosMap_API.Dtos;
using MosMap_API.Helpers;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;
using SQLitePCL;

namespace MosMap_API.Services
{
    public class PhotoService : IPhotosService
    {
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        private IMapper _mapper;
        private Cloudinary _cloudinary;
        private DataContext _context;

        public PhotoService(IOptions<CloudinarySettings> cloudinaryConfig, IMapper mapper, DataContext context)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _context = context;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        
        public async Task<PhotoForReturnDto> AddPhotoForLocation(int locationId, PhotoForCreationDto photoForCreationDto)
        {
            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);
            
            photo.IsMain = true;
            await _context.Photos.AddAsync(photo);
            
            
            await _context.SaveChangesAsync();
            
            var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
            return photoToReturn;
        }

        public async Task<PhotoForReturnDto> GetPhoto(int id)
        {
            var photoFromDb = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromDb);
            return photo;
        }
    }
}