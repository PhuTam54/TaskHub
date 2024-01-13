using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskHub.Models;

namespace TaskHub.Data
{
    public class TaskHubContext : DbContext
    {
        public TaskHubContext (DbContextOptions<TaskHubContext> options)
            : base(options)
        {
        }

        public DbSet<TaskHub.Models.TaskItem> TaskItem { get; set; } = default!;
    }
}
