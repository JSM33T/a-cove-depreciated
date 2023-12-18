using System.ComponentModel.DataAnnotations;

namespace almondcove.Models.DTO
{
    public class MailDTO
    {
        [Required]
        [EmailAddress]
        [StringLength(100, MinimumLength = 5)]
        public string EMail { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Origin { get; set; }
    }
}