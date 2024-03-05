using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskHub.Models;

namespace TaskHub.Models
{
    public class User
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        public string Password { get; set; }
        [DefaultValue(false)]
        public bool IsAdmin { get; set; }
        [StringLength(50)]
        public string UserRole { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiration { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstMidName + " " + LastName;
            }
        }

        public ICollection<WorkSpaceMember> WorkSpaceMembers { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<TaskItem> TaskItems { get; set; }
        public ICollection<WorkSpace> WorkSpaces { get; set; }
    }
}