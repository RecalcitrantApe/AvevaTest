using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Application.Exceptions;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Domain.ValueObjects;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool CurrentlyEmployed { get; set; }

    }

    //Let's have the handler at the same level to make it easy to find

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IEmployeeDbContext context;

        public UpdateEmployeeCommandHandler(IEmployeeDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Employees.FindAsync(request.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(Employee), request.Id);
            }

            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.DateOfBirth = DateOnly.From(request.DateOfBirth);
            entity.Email = Email.From(request.Email);
            entity.CurrentlyEmployed = request.CurrentlyEmployed;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
