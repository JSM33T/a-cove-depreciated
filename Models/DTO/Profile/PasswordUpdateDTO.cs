using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.DTO.Profile
{
    public class PasswordUpdateDTO
    {
        [Required]
        public string Password { get; set; }
    }
}
