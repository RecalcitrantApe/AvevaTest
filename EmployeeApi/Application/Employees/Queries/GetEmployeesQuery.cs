//using AutoMapper;
//using EmployeeApi.Contexts;
//using EmployeeApi.Model;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using AutoMapper.QueryableExtensions;

//namespace EmployeeApi.Application.Employees.Queries
//{
//    public class GetEmployeesQuery : IRequest<IList<EmployeeDto>>
//    {
     
//    }

//    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IList<EmployeeDto>>
//    {
//        private readonly IEmployeeDbContext context;
//        private readonly IMapper mapper;

//        public GetEmployeesQueryHandler(IEmployeeDbContext context, IMapper mapper)
//        {
//            this.context = context;
//            this.mapper = mapper;
//        }

//        public async Task<IList<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
//        {

//            //var list = await context.Employees.AsNoTracking().ToListAsync(cancellationToken)

//            var list = await context.Employees.Include(x => x.Computers).AsNoTracking()
//                .ProjectTo<EmployeeDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);

//            return list;
//        }
//    }
//}
