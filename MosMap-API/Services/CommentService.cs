using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MosMap_API.Data;
using MosMap_API.Dtos;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Services
{
    public class CommentService : ICommentService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private IMapper _mapper;
        private IUserService _uservice;

        public CommentService(IConfiguration config, DataContext context, IMapper mapper, IUserService uservice)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
            _uservice = uservice;
        }

        public async Task<Comment> CreateComment(CommentForCreationDto commentDto)
        {
            Comment comment = new Comment()
            {
                CommentText = commentDto.CommentText,
                CreatedDate = DateTime.Now,
                User = _context.ApplicationUser.FirstOrDefault(i => i.Id.Equals(_uservice.GetUserId())),
                Location = _context.Locations.FirstOrDefault(i => i.Id.Equals(commentDto.LocationId))
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public void DeleteComment(Comment comment)
        {
            _context.Remove(comment);
            _context.SaveChanges();
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _context.Comments
                .Include(i => i.User)
                .Include(i => i.Location)
                .FirstOrDefaultAsync(i => i.Id.Equals(id));
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByLocaitonId(int locationId)
        {
            List<Comment> comments =  await _context.Comments
                .Where(i => i.Location.Id.Equals(locationId))
                .Include(i => i.User)
                .Include(i => i.Location)
                .ToListAsync();

            List<CommentDto> commentsResult = _mapper.Map<List<CommentDto>>(comments);
            commentsResult.ForEach(i =>
            {
                i.UserName = _context.ApplicationUser
                .Where(j => j.Id.Equals(i.UserId))
                .Select(j => j.Username)
                .FirstOrDefault();
            });

            return commentsResult;
        }
    }
}
