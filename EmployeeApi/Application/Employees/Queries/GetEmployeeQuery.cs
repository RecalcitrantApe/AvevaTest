using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using EmployeeApi.Application.Exceptions;
using EmployeeApi.Application.Common.Interfaces;

namespace EmployeeApi.Application.Employees.Queries
{
    public class GetEmployeeQuery : IRequest<EmployeeDto>
    {
        public int Id { get; private set; }

        public GetEmployeeQuery(int id)
        {
            Id = id;
        }

    }

    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeDto>
    {
        private readonly IEmployeeDbContext context;
        private readonly IMapper mapper;
        public GetEmployeeQueryHandler(IEmployeeDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employee = await context.Employees.Include(x => x.Computers).AsNoTracking().ProjectTo<EmployeeDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == request.Id);

            if (employee == null)
                throw new EntityNotFoundException("Not found");

            return employee;
        }


    }

}
