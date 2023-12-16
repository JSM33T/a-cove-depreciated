using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.Domain
{
    public class UserProfile
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
        
        public int AvatarId { get; set; }
        
        public string Bio { get; set; }

        [Required]
        public DateTime DateJoined { get; set; }
        
        public string DateElement { get; set; }
        
        public DateTime DateEdited { get; set; }

        [Required]
        public string Role { get; set; }
        
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }
        
        [MinLength(6)]
        [MaxLength(20)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string AvatarImg { get; set; }
        
        public string Badges { get; internal set; }
    }
}
