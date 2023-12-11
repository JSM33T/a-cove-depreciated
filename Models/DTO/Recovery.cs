using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.DTO
{
    public class Recovery
    {
        [Required]
        public string UserName { get; set; }
    }
}
