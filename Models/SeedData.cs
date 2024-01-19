using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Data;
using System;
using System.Linq;
using ContosoUniversity.Models;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using NuGet.Packaging.Signing;

namespace TaskHub.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new TaskHubContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<TaskHubContext>>()))
        {
            // Look for any movies.
            if (context.TaskItem.Any())
            {
                return;   // DB has been seeded
            }
            var Users = new User[]
            {
                new User
                {
                    UserName = "phutam",
                    Email = "phutam@gamil.com",
                    Password = "123456",
                    FirstMidName = "Tam",
                    LastName = "Phu",
                    EnrollmentDate = DateTime.Parse("2024-01-01")
                },
                new User
                {
                    UserName = "tranthuy",
                    Email = "tranthuy@gamil.com",
                    Password = "123456",
                    FirstMidName = "Thuy",
                    LastName = "Tran",
                    EnrollmentDate = DateTime.Parse("2024-01-01")
                }
            };
            foreach (User s in Users)
            {
                context.User.Add(s);
            }
            context.SaveChanges();

            var WorkSpaces = new WorkSpace[]
             {
                new WorkSpace
                {
                    WorkSpaceTitle = "WorkSpace number 1",
                    WorkSpaceDescription = "Description For WorkSpace number 1",
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    Status = 1,
                },
                new WorkSpace
                {
                    WorkSpaceTitle = "WorkSpace number 2",
                    WorkSpaceDescription = "Description For WorkSpace number 2",
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    Status = 2,
                }
            };
            foreach (WorkSpace s in WorkSpaces)
            {
                context.WorkSpace.Add(s);
            }
            context.SaveChanges();

            var Boards = new Board[]
             {
                new Board
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "WorkSpace number 1").WorkSpaceId,
                    BoardTitle = "Board number 1",
                    Status = 1,
                },
                new Board
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "WorkSpace number 1").WorkSpaceId,
                    BoardTitle = "Board number 2",
                    Status = 2,
                }
            };
            foreach (Board s in Boards)
            {
                context.Board.Add(s);
            }
            context.SaveChanges();

            var Lists = new List[]
             {
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "Board number 1").BoardId,
                    ListTitle = "Todo",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "Board number 1").BoardId,
                    ListTitle = "Done",
                    Status = 1,
                }
             };
            foreach (List s in Lists)
            {
                context.List.Add(s);
            }
            context.SaveChanges();

            var TaskItems = new TaskItem[]
             {
                new TaskItem
                {
                    Title = "TaskItem number 1",
                    Description = "Description For TaskItem number 1",
                    Deadline = DateTime.Parse("2024-1-20"),
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Todo").ListId,
                    position = 1,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "TaskItem number 2",
                    Description = "Description For TaskItem number 2",
                    Deadline = DateTime.Parse("2024-1-21"),
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Done").ListId,
                    position = 1,
                    Status = 1,
                }
            };
            foreach (TaskItem s in TaskItems)
            {
                context.TaskItem.Add(s);
            }
            context.SaveChanges();

            var Comments = new Comment[]
             {
                new Comment
                {
                    CommentContent = "Comment number 1",
                    Timestamp = DateTime.Now,
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    TaskItemId = TaskItems.Single( s => s.Title == "TaskItem number 1").Id,
                    Status = 1,
                },
                new Comment
                {
                    CommentContent = "Comment number 2",
                    Timestamp = DateTime.Now,
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    TaskItemId = TaskItems.Single( s => s.Title == "TaskItem number 2").Id,
                    Status = 1,
                }
             };
            foreach (Comment s in Comments)
            {
                context.Comment.Add(s);
            }
            context.SaveChanges();

            var WorkSpaceMembers = new WorkSpaceMember[]
             {
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "WorkSpace number 1").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "WorkSpace number 1").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "tranthuy").ID,
                    Status = 1,
                }
            };
            foreach (WorkSpaceMember s in WorkSpaceMembers)
            {
                context.WorkSpaceMember.Add(s);
            }
            context.SaveChanges();
        }
    }
}