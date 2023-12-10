using System.ComponentModel.DataAnnotations;

namespace laymaann.Models.DTO
{
    public class Recovery
    {
        [Required]
        public string UserName { get; set; }
    }
}
