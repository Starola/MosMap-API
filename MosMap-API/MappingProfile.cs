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
            CreateMap<Location, LocationDto>();

            CreateMap<Location, LocationForAdminDto>();

            // Comment:
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentForCreationDto, Comment>();

        }
    }
}
