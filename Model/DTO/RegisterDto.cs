using System.ComponentModel.DataAnnotations;

namespace DatingApi.Model.DTO
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
