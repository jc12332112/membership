using System.ComponentModel.DataAnnotations;

namespace membership.Models
{
    public class RequestPasswordChangeModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
