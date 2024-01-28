﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskHub.Data;

#nullable disable

namespace TaskHub.Migrations
{
    [DbContext(typeof(TaskHubContext))]
    [Migration("20240127220550_AddAvatarFeild")]
    partial class AddAvatarFeild
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskHub.Models.Board", b =>
                {
                    b.Property<int>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BoardId"));

                    b.Property<string>("BoardTitle")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("WorkSpaceId")
                        .HasColumnType("int");

                    b.HasKey("BoardId");

                    b.HasIndex("WorkSpaceId");

                    b.ToTable("Board", (string)null);
                });

            modelBuilder.Entity("TaskHub.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("CommentContent")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TaskItemId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("TaskItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Comment", (string)null);
                });

            modelBuilder.Entity("TaskHub.Models.List", b =>
                {
                    b.Property<int>("ListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ListId"));

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<string>("ListTitle")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ListId");

                    b.HasIndex("BoardId");

                    b.ToTable("List", (string)null);
                });

            modelBuilder.Entity("TaskHub.Models.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("ListId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("position")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.HasIndex("UserId");

                    b.ToTable("TaskItem", (string)null);
                });

            modelBuilder.Entity("TaskHub.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstMidName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("TaskHub.Models.WorkSpace", b =>
                {
                    b.Property<int>("WorkSpaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkSpaceId"));

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("WorkSpaceDescription")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("WorkSpaceTitle")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("WorkSpaceId");

                    b.HasIndex("UserId");

                    b.ToTable("WorkSpace", (string)null);
                });

            modelBuilder.Entity("TaskHub.Models.WorkSpaceMember", b =>
                {
                    b.Property<int>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemberId"));

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WorkSpaceId")
                        .HasColumnType("int");

                    b.HasKey("MemberId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkSpaceId");

                    b.ToTable("WorkSpaceMember", (string)null);
                });

            modelBuilder.Entity("TaskHub.Models.Board", b =>
                {
                    b.HasOne("TaskHub.Models.WorkSpace", "WorkSpace")
                        .WithMany("Boards")
                        .HasForeignKey("WorkSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkSpace");
                });

            modelBuilder.Entity("TaskHub.Models.Comment", b =>
                {
                    b.HasOne("TaskHub.Models.TaskItem", "TaskItem")
                        .WithMany("Comments")
                        .HasForeignKey("TaskItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskHub.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskHub.Models.List", b =>
                {
                    b.HasOne("TaskHub.Models.Board", "Board")
                        .WithMany("Lists")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("TaskHub.Models.TaskItem", b =>
                {
                    b.HasOne("TaskHub.Models.List", "List")
                        .WithMany("TaskItems")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskHub.Models.User", "User")
                        .WithMany("TaskItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskHub.Models.WorkSpace", b =>
                {
                    b.HasOne("TaskHub.Models.User", "User")
                        .WithMany("WorkSpaces")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskHub.Models.WorkSpaceMember", b =>
                {
                    b.HasOne("TaskHub.Models.User", "User")
                        .WithMany("WorkSpaceMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskHub.Models.WorkSpace", "WorkSpace")
                        .WithMany("WorkSpaceMembers")
                        .HasForeignKey("WorkSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WorkSpace");
                });

            modelBuilder.Entity("TaskHub.Models.Board", b =>
                {
                    b.Navigation("Lists");
                });

            modelBuilder.Entity("TaskHub.Models.List", b =>
                {
                    b.Navigation("TaskItems");
                });

            modelBuilder.Entity("TaskHub.Models.TaskItem", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("TaskHub.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("TaskItems");

                    b.Navigation("WorkSpaceMembers");

                    b.Navigation("WorkSpaces");
                });

            modelBuilder.Entity("TaskHub.Models.WorkSpace", b =>
                {
                    b.Navigation("Boards");

                    b.Navigation("WorkSpaceMembers");
                });
#pragma warning restore 612, 618
        }
    }
}