using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Data;
using System;
using System.Linq;

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
            context.TaskItem.AddRange(
                new TaskItem
                {
                    Title = "TaskItem number 1",
                    Description = "Description For TaskItem number 1",
                    Deadline = DateTime.Parse("2024-1-15"),
                    UserId = 2,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "TaskItem number 2",
                    Description = "Description For TaskItem number 2",
                    Deadline = DateTime.Parse("2024-1-15"),
                    UserId = 3,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "TaskItem number 3",
                    Description = "Description For TaskItem number 3",
                    Deadline = DateTime.Parse("2024-1-15"),
                    UserId = 2,
                    Status = 3,
                },
                new TaskItem
                {
                    Title = "TaskItem number 4",
                    Description = "Description For TaskItem number 4",
                    Deadline = DateTime.Parse("2024-1-15"),
                    UserId = 1,
                    Status = 1,
                },
                new TaskItem
                {
                    Title = "TaskItem number 5",
                    Description = "Description For TaskItem number 5",
                    Deadline = DateTime.Parse("2024-1-15"),
                    UserId = 1,
                    Status = 2,
                }
            );
            context.SaveChanges();
        }
    }
}