using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Application.Exceptions;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Domain.ValueObjects;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool CurrentlyEmployed { get; set; }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IEmployeeDbContext context;

        public CreateEmployeeCommandHandler(IEmployeeDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = context.Employees.FirstOrDefault(x => x.Email.EmailString == request.Email);

            if (entity != null)
            {
                throw new EntityAlreadyPresentException(nameof(Employee), request.Email);
            }

            //Only add the top entity
            entity = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = DateOnly.From(request.DateOfBirth),
                Email = Email.From(request.Email),
                CurrentlyEmployed = request.CurrentlyEmployed
            };

            context.Employees.Add(entity);

            await context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
