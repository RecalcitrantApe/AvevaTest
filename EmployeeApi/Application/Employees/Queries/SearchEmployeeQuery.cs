using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Application.Common.Interfaces;

namespace EmployeeApi.Application.Employees.Queries
{
    public class SearchEmployeeQuery : IRequest<IList<EmployeeDto>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool FilterOnIsEmployed { get; set; } = false;
        public bool IsEmployed { get; set; }
    }

    public class SearchEmployeeQueryHandler : IRequestHandler<SearchEmployeeQuery, IList<EmployeeDto>>
    {
        private readonly IEmployeeDbContext context;
        private readonly IMapper mapper;

        public SearchEmployeeQueryHandler(IEmployeeDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IList<EmployeeDto>> Handle(SearchEmployeeQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Employee> query = context.Employees.Include(x => x.Computers);

            if (!string.IsNullOrEmpty(request.FirstName))
                query = query.Where(x => x.FirstName.Contains(request.FirstName));

            if (!string.IsNullOrEmpty(request.LastName))
                query = query.Where(x => x.LastName.Contains(request.LastName));

            if (!string.IsNullOrEmpty(request.Email))
                query = query.Where(x => x.Email.EmailString.Contains(request.Email));

            if (request.FilterOnIsEmployed)
                query = query.Where(x => x.CurrentlyEmployed == request.IsEmployed);


            var list = await query.AsNoTracking()
            .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

            return list;
        }
    }
}
