using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductiveBoard.Models;

namespace ProductiveBoard.Data
{
    public static class DbSeed
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole("Admin")
                {
                    Id = "1",
                    NormalizedName = "Admin"
                });
                context.Roles.Add(new IdentityRole("User")
                {
                    Id = "2",
                    NormalizedName = "User"
                });
            }

            if (!context.TaskTypes.Any())
            {
                context.TaskTypes.Add(new TaskType()
                {
                    Name = "Fix"
                });
                context.TaskTypes.Add(new TaskType()
                {
                    Name = "Feature"
                });
                context.TaskTypes.Add(new TaskType()
                {
                    Name = "Chore"
                });
                context.TaskTypes.Add(new TaskType()
                {
                    Name = "Refactor"
                });

                context.TaskTypes.Add(new TaskType()
                {
                    Name = "Design"
                });
            }

            if (!context.TaskStatuses.Any())
            {
                context.TaskStatuses.Add(new Models.TaskStatus()
                {
                    Name = "TODO"
                });
                context.TaskStatuses.Add(new Models.TaskStatus()
                {
                    Name = "In Progress"
                });
                context.TaskStatuses.Add(new Models.TaskStatus()
                {
                    Name = "Done"
                });

                context.TaskStatuses.Add(new Models.TaskStatus()
                {
                    Name = "Backlog"
                });

                context.TaskStatuses.Add(new Models.TaskStatus()
                {
                    Name = "Code Review"
                });
            }

            context.SaveChanges();
        }
    }
}
