using MosMap_API.Dtos;
using MosMap_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.ServiceInterfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetCommentsByLocaitonId(int locationId);
        Task<Comment> GetCommentById(int id);
        Task<Comment> CreateComment(CommentForCreationDto commentDto);
        void DeleteComment(Comment comment);

    }
}
