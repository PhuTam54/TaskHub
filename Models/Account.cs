using System.ComponentModel.DataAnnotations;

namespace TaskHub.Models
{
    public class Account
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
