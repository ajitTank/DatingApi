using System.ComponentModel.DataAnnotations;

namespace DatingApi.Model.DTO
{
    public class LoginDto
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
