using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Data;
using System;
using System.Linq;
using TaskHub.Models;
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
                    Email = "phutamytb@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Tam",
                    LastName = "Phu",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "tranthuy",
                    Email = "tranthuy@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Thuy",
                    LastName = "Tran",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "peter",
                    Email = "peter@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Peter",
                    LastName = "Parker",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "tom",
                    Email = "tamnpth2210002@fpt.edu.vn",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Tom",
                    LastName = "Holland",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "ronaldo",
                    Email = "ronaldo@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Cristiano",
                    LastName = "Ronaldo",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "phutam2",
                    Email = "phutam2@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Tam",
                    LastName = "Phu 2",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "tranthuy2",
                    Email = "tranthuy2@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Thuy",
                    LastName = "Tran 2",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "peter2",
                    Email = "peter2@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Peter",
                    LastName = "Parker 2",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "tom2",
                    Email = "tom2@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Tom",
                    LastName = "Holland 2",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "ronaldo2",
                    Email = "ronaldo2@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Cristiano",
                    LastName = "Ronaldo 2",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "phutam3",
                    Email = "phutam3@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Tam",
                    LastName = "Phu 3",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "tranthuy3",
                    Email = "tranthuy3@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Thuy",
                    LastName = "Tran 3",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "peter3",
                    Email = "peter3@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Peter",
                    LastName = "Parker 3",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "tom3",
                    Email = "tom3@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Tom",
                    LastName = "Holland 3",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
                },
                new User
                {
                    UserName = "ronaldo3",
                    Email = "ronaldo3@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    FirstMidName = "Cristiano",
                    LastName = "Ronaldo 3",
                    Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg",
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
                    WorkSpaceTitle = "Group1's workspace",
                    WorkSpaceDescription = "Description For Group1's workspace",
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    Status = 1,
                },
                new WorkSpace
                {
                    WorkSpaceTitle = "Group2's workspace",
                    WorkSpaceDescription = "Description For Group2's workspace",
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    Status = 2,
                },
                new WorkSpace
                {
                    WorkSpaceTitle = "Group3's workspace",
                    WorkSpaceDescription = "Description For WorkSpace number 3",
                    UserId = Users.Single( s => s.UserName == "ronaldo").ID,
                    Status = 1,
                },
                new WorkSpace
                {
                    WorkSpaceTitle = "Group4's workspace",
                    WorkSpaceDescription = "Description For WorkSpace number 4",
                    UserId = Users.Single( s => s.UserName == "tranthuy").ID,
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
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group1's workspace").WorkSpaceId,
                    BoardTitle = "TaskHub",
                    Status = 1,
                },
                new Board
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group1's workspace").WorkSpaceId,
                    BoardTitle = "MvcMovie",
                    Status = 2,
                },
                new Board
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group1's workspace").WorkSpaceId,
                    BoardTitle = "ContosoUniversity",
                    Status = 3,
                },
                new Board
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group2's workspace").WorkSpaceId,
                    BoardTitle = "Taskhub",
                    Status = 3,
                },
                new Board
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group2's workspace").WorkSpaceId,
                    BoardTitle = "Mvcmovie",
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
                    BoardId = Boards.Single( s => s.BoardTitle == "TaskHub").BoardId,
                    ListTitle = "Todo",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "TaskHub").BoardId,
                    ListTitle = "Doing",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "TaskHub").BoardId,
                    ListTitle = "Done",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "Taskhub").BoardId,
                    ListTitle = "Done 2",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "Taskhub").BoardId,
                    ListTitle = "Todo 2",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "MvcMovie").BoardId,
                    ListTitle = "Done 3",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "MvcMovie").BoardId,
                    ListTitle = "Doing 3",
                    Status = 1,
                },
                new List
                {
                    BoardId = Boards.Single( s => s.BoardTitle == "MvcMovie").BoardId,
                    ListTitle = "Todo 3",
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
                    Title = "Clone project on Github",
                    Description = "Description For TaskItem number 1",
                    Deadline = DateTime.Parse("2024-1-20"),
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Done").ListId,
                    position = 1,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "Database design",
                    Description = "Description For TaskItem number 2",
                    Deadline = DateTime.Parse("2024-1-21"),
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Done").ListId,
                    position = 0,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "UX/UI ( Dashboard )",
                    Description = "Description For TaskItem number 3",
                    Deadline = DateTime.Parse("2024-1-22"),
                    UserId = Users.Single( s => s.UserName == "tranthuy").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Todo").ListId,
                    position = 0,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "Add template",
                    Description = "Description For TaskItem number 4",
                    Deadline = DateTime.Parse("2024-1-21"),
                    UserId = Users.Single( s => s.UserName == "peter").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Doing").ListId,
                    position = 0,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "UX/UI ( User )",
                    Description = "Description For TaskItem number 5",
                    Deadline = DateTime.Parse("2024-1-20"),
                    UserId = Users.Single( s => s.UserName == "tom").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Todo").ListId,
                    position = 0,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "Clone project on Github.com",
                    Description = "Description For TaskItem number 6",
                    Deadline = DateTime.Parse("2024-1-21"),
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Done 2").ListId,
                    position = 0,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "UX/UI for the Products page",
                    Description = "Description For TaskItem number 7",
                    Deadline = DateTime.Parse("2024-1-20"),
                    UserId = Users.Single( s => s.UserName == "tom").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Todo 2").ListId,
                    position = 0,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "UX/UI ( Admin )",
                    Description = "Description For TaskItem number 8",
                    Deadline = DateTime.Parse("2024-1-22"),
                    UserId = Users.Single( s => s.UserName == "peter").ID,
                    ListId = Lists.Single( s => s.ListTitle == "Done 3").ListId,
                    position = 0,
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
                    CommentContent = "Imediately",
                    Timestamp = DateTime.Now,
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    TaskItemId = TaskItems.Single( s => s.Title == "Clone project on Github").Id,
                    Status = 1,
                },
                new Comment
                {
                    CommentContent = "Good",
                    Timestamp = DateTime.Now,
                    UserId = Users.Single( s => s.UserName == "phutam").ID,
                    TaskItemId = TaskItems.Single( s => s.Title == "Database design").Id,
                    Status = 1,
                },
                new Comment
                {
                    CommentContent = "Well done!",
                    Timestamp = DateTime.Now,
                    UserId = Users.Single( s => s.UserName == "tranthuy").ID,
                    TaskItemId = TaskItems.Single( s => s.Title == "Clone project on Github").Id,
                    Status = 1,
                },
                new Comment
                {
                    CommentContent = "Yes",
                    Timestamp = DateTime.Now,
                    UserId = Users.Single( s => s.UserName == "peter").ID,
                    TaskItemId = TaskItems.Single( s => s.Title == "Database design").Id,
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
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group1's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "ronaldo").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group1's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "tranthuy").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group1's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "peter").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group1's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "tom").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group2's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "ronaldo2").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 2,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group3's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "ronaldo3").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group2's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "tranthuy2").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group3's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "peter2").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group3's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "tom2").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 1,
                },
                new WorkSpaceMember
                {
                    WorkSpaceId = WorkSpaces.Single( s => s.WorkSpaceTitle == "Group4's workspace").WorkSpaceId,
                    UserId = Users.Single( s => s.UserName == "ronaldo3").ID,
                    EnrollmentDate = DateTime.Now,
                    Status = 2,
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