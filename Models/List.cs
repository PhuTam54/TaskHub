using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskHub.Models;

public class List
{
    public int ListId { get; set; }

    public int BoardId { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? ListTitle { get; set; }

    [Range(1, 3)]
    public int Status { get; set; }
    //(1.Todo, 2.Doing, 3.Done, ...)
    public Board Board { get; set; }
    public ICollection<TaskItem> TaskItems { get; set; }
}