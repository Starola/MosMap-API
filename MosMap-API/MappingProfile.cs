using AutoMapper;
using MosMap_API.Dtos;
using MosMap_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category:
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryForCreationDto, Category>();
            CreateMap<CategoryForUpdateDto, Category>();

            // Subcategory:
            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<SubCategory, SubCategoryWithDetailsDto>();
            CreateMap<SubCategoryForCreationDto, SubCategory>();
            CreateMap<SubCategoryForUpdateDto, SubCategory>();

            // Location:
            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<Location, LocationForAdminDto>();
            CreateMap<Location, LocationByCategoryDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));

            // Comment:
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentForCreationDto, Comment>();
            
            //Photo:
            CreateMap<Photo, PhotoForDetailDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDto>();

        }
    }
}
