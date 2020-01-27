using System.Collections.Generic;

namespace MosMap_API.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public virtual Authorization Authorization { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}