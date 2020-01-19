using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MosMap_API.Models
{
    public class Authorization
    {
        public int Id { get; set; }
        public string Role { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
