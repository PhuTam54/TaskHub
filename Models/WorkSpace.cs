using ContosoUniversity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskHub.Models;

public class WorkSpace
{
    public int WorkSpaceId { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? WorkSpaceTitle { get; set; }

    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? WorkSpaceDescription { get; set; }

    [Range(1, 3)]
    public int Status { get; set; }
    //(1.Public, 2.Private, ...)

    [Required]
    [StringLength(50)]
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<WorkSpaceMember> WorkSpaceMembers { get; set; }
    public ICollection<Board> Boards { get; set; }
}