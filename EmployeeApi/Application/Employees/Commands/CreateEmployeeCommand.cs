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
            entity = new Employee();

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.DateOfBirth = DateOnly.From(request.DateOfBirth);
            entity.Email = Email.From(request.Email);
            entity.CurrentlyEmployed = request.CurrentlyEmployed;

            context.Employees.Add(entity);

            //context.Employees.Add(new Entities.Employee
            //{
            //    FirstName = "EmployeeTwo",
            //    LastName = "TwoSon",
            //    Email = Email.From("emplo1yee2@org.com"),
            //    DateOfBirth = DateOnly.From(new DateTime(1995, 1, 1)),
            //    CurrentlyEmployed = true,

            //    Computers =
            //        {
            //            new Entities.Computer { Name = "Dell", Description = "Nice Dell1" },
            //            new Entities.Computer { Name = "Dell", Description = "Nice Dell2" },
            //        }
            //});


            await context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
