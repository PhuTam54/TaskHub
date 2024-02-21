using System.ComponentModel.DataAnnotations;

namespace TaskHub.Models
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
