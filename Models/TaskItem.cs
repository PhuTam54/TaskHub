using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskHub.Models;

public class TaskItem
{
    public int Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Title { get; set; }

    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Deadline")]
    public DateTime Deadline { get; set; }

    public int UserId { get; set; } //(Foreign Key): Liên kết với người dùng thực hiện công việc.

    [Range(1, 3)]
    public int Status { get; set; }
    //(1.đang thực hiện, 2.hoàn thành, 3.chờ xác nhận, ...)
}