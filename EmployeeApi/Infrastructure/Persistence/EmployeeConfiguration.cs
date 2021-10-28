using EmployeeApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Persistence
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        //https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(t => t.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(t => t.LastName)
                .HasMaxLength(150)
                .IsRequired();

            builder
              .OwnsOne(b => b.DateOfBirth)
              .Property(p => p.Date).IsRequired();

            builder
              .OwnsOne(b => b.DateOfBirth)
              .Ignore(x => x.Year);

            builder
              .OwnsOne(b => b.DateOfBirth)
              .Ignore(x => x.Month);

            builder
              .OwnsOne(b => b.DateOfBirth)
              .Ignore(x => x.Day);



            builder
              .OwnsOne(b => b.Email)
              .Property(p => p.EmailString).IsRequired().HasMaxLength(200);



            builder.Property(t => t.CurrentlyEmployed)
                .IsRequired();

            builder.Property(t => t.CreatedBy)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.LastModifiedBy)
                .HasMaxLength(100);
        }
    }
    
}
