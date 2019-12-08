using System.ComponentModel.DataAnnotations;

namespace MosMap_API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Das Passwort muss zwischen 4 und 16 Zeichen haben")]
        public string Password { get; set; }
    }
}