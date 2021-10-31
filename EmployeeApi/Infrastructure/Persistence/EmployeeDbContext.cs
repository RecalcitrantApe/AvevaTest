using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Persistence
{
    public class EmployeeDbContext : DbContext, IEmployeeDbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Computer> Computers { get; set; }

        private ICurrentUserService currentUserService { get; set; }


        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            this.currentUserService = currentUserService;
            //Database.EnsureCreated();
        }
        public EmployeeDbContext(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AbstractAuditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUserService.UserId;
                        entry.Entity.Created = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = currentUserService.UserId;
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
