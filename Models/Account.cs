using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskHub.Models
{
    [Table("Account")]
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

    }
}
