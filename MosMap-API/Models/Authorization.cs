using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Models
{
    public class Authorization
    {
        public int Id { get; set; }
        public bool Admin { get; set; }
        public bool Council { get; set; }
        public bool User { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
