using TaskHub.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskHub.Models;

public class WorkSpaceMember
{
    [Key]
    public int MemberId { get; set; }

    public int WorkSpaceId { get; set; }
    public int UserId { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; set; }

    [Range(1, 3)]
    public int Status { get; set; }
    //(1.Active, 2.Inactive, ...)
    public User User { get; set; }
    public WorkSpace WorkSpace { get; set; }
}