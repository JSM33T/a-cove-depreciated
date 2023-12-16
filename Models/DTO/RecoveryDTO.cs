using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.DTO
{
    public class RecoveryDTO
    {
        [Required]
        public string UserName { get; set; }
    }
}
