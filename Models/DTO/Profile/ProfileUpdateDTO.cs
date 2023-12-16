using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.DTO.Profile
{
    public class ProfileUpdateDTO
    {
        [Required]
        public int Id { get; internal set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string EMail { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        [Required]
        public int AvatarId { get; set; }

        public string Bio { get; set; }

        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
