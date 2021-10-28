using EmployeeApi.Domain.Entities;
using EmployeeApi.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Persistence
{
    public class EmployeeDbContextSeed
    {
        public static async Task SeedSampleDataAsync(EmployeeDbContext context)
        {
            // Seed, if necessary
            if (!context.Employees.Any())
            {
                context.Employees.Add(new Employee
                {
                    FirstName = "EmployeeOne",
                    LastName = "OneSon",
                    Email = Email.From("employee1@org.com"),
                    DateOfBirth = DateOnly.From(new DateTime(1995,1,1)),
                    CurrentlyEmployed = true,
                    
                    Computers =
                    {
                        new Computer { Name = "Lenovo", Description = "Nice Lenovo" },
                        new Computer { Name = "Lenovo2", Description = "Nice Lenovo2" },
                    }
                });

                context.Employees.Add(new Employee
                {
                    FirstName = "EmployeeTwo",
                    LastName = "TwoSon",
                    Email = Email.From("employee2@org.com"),
                    DateOfBirth = DateOnly.From(new DateTime(1995, 1, 1)),
                    CurrentlyEmployed = true,

                    Computers =
                    {
                        new Computer { Name = "Dell", Description = "Nice Dell1" },
                        new Computer { Name = "Dell", Description = "Nice Dell2" },
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
