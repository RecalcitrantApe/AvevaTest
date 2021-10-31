using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Application.Employees.Queries;
using EmployeeApi.Application.Exceptions;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Employees.Commands
{
    public class PatchEmployeeCommand : IRequest
    {
        public int Id { get; set; }

        public JsonPatchDocument<EmployeeDto> Patch { get; set; }

    }

    //Let's have the handler at the same level to make it easy to find

    public class PatchEmployeeCommandHandler : IRequestHandler<PatchEmployeeCommand>
    {
        private readonly IEmployeeDbContext context;
        private readonly IMapper mapper;

        public PatchEmployeeCommandHandler(IEmployeeDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(PatchEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.Employees.FindAsync(request.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(Employee), request.Id);
            }
            try
            {
                var employeeToPatch = await context.Employees.Include(x => x.Computers).AsNoTracking().ProjectTo<EmployeeDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == request.Id);

                request.Patch.ApplyTo(employeeToPatch);

                PatchEmployeeCommandValidator validator = new PatchEmployeeCommandValidator();

                var result = validator.Validate(employeeToPatch);

                if (!result.IsValid)
                {
                    throw new EntityInvalidException("Invalid state");
                }

                //Object is valid, let's save. Maybe should use AutoMapper here, but it is getting late and I am tired.
                entity.DateOfBirth = DateOnly.From(employeeToPatch.DateOfBirth);
                entity.CurrentlyEmployed = employeeToPatch.CurrentlyEmployed;
                entity.FirstName = employeeToPatch.FirstName;
                entity.LastName = employeeToPatch.LastName;
                entity.Email = Email.From(employeeToPatch.Email);

                context.Employees.Attach(entity);

                await context.SaveChangesAsync(cancellationToken);

            }
            catch (Exception)
            {
                throw new EntityInvalidException("Invalid state");
            }

            return Unit.Value;
        }
    }
}
