using System.ComponentModel.DataAnnotations;

namespace almondCove.Models.DTO
{
    public class Recovery
    {
        [Required]
        public string UserName { get; set; }
    }
}
