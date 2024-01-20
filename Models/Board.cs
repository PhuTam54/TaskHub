using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskHub.Models;

public class Board
{
    public int BoardId { get; set; }

    public int WorkSpaceId { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? BoardTitle { get; set; }

    [Range(1, 3)]
    public int Status { get; set; }
    //(1.Public, 2.Private, ...)
    public WorkSpace WorkSpace { get; set; }
    public ICollection<List> Lists { get; set; }

}