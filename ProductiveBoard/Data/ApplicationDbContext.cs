﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMan.Models;

namespace TaskMan.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Task>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Task>()
                .HasOne(e => e.Status)
                .WithMany()
                .HasForeignKey(e => e.StatusId);

            modelBuilder.Entity<TaskStatus>()
            .HasKey(e => e.Id);
            modelBuilder.Entity<TaskStatus>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        }
    }
}
