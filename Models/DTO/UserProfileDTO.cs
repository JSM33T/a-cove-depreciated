using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.DTO
{
    public class UserProfileDTO
    {

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
