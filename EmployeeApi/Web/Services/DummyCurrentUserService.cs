using EmployeeApi.Application.Common.Interfaces;

namespace EmployeeApi.Web.Services
{
    public class DummyCurrentUserService : ICurrentUserService
    {
        public string UserId => "DummyUser";
    }
}
