using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskHub.Models;
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

        public DbSet<TaskItem> TaskItem { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<WorkSpace> WorkSpace { get; set; } = default!;
        public DbSet<Board> Board { get; set; } = default!;
        public DbSet<List> List { get; set; } = default!;
        public DbSet<WorkSpaceMember> WorkSpaceMember { get; set; } = default!;
        public DbSet<Comment> Comment { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>().ToTable("TaskItem");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<WorkSpace>().ToTable("WorkSpace");
            modelBuilder.Entity<Board>().ToTable("Board");
            modelBuilder.Entity<List>().ToTable("List");
            modelBuilder.Entity<WorkSpaceMember>().ToTable("WorkSpaceMember");
            modelBuilder.Entity<Comment>().ToTable("Comment");
        }
    }
}
