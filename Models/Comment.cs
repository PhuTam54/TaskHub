using TaskHub.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskHub.Models;

public class Comment
{
    public int CommentId { get; set; }

    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? CommentContent { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Timestamp")]
    public DateTime Timestamp { get; set; }

    public int UserId { get; set; } //(Foreign Key): Liên kết với người dùng thực hiện công việc.
    public int TaskItemId { get; set; } //(Foreign Key): Liên kết với người dùng thực hiện công việc.

    [Range(1, 3)]
    public int Status { get; set; }
    //(1.đang thực hiện, 2.hoàn thành, 3.chờ xác nhận, ...)

    public User User { get; set; }
    public TaskItem TaskItem { get; set; }
}