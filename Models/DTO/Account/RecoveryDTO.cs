using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.DTO.Account
{
    public class RecoveryDTO
    {
        [Required]
        public string UserName { get; set; }
    }
}
