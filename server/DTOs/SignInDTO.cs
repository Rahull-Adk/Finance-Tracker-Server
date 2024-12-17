using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class SignInDTO
    {
        [Required]
        public string EmailOrUsername { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
