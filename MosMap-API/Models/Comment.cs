using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Location Location { get; set; }

    }
}
