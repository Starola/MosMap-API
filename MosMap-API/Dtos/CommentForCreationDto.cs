using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Dtos
{
    public class CommentForCreationDto
    {
        public string CommentText { get; set; }

        public int LocationId { get; set; }
    }
}
