using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public int LocationId { get; set; }
    }
}
