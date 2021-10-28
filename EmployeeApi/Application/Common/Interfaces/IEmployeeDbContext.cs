using EmployeeApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Common.Interfaces
{
    public interface IEmployeeDbContext
    {
        DbSet<Employee> Employees { get; set; }

        DbSet<Computer> Computers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);


    }
}
