using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Web.Services
{

    public static class StartupExtensions
    {
        public static IApplicationBuilder Use401ForUnauthorizedCalls(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
         


                //if ((context.Request.Path.Value.Contains("/api") && !context.Request.Path.Value.Contains("/signin")) && 
                //    (context.User == null ||
                //    context.User.Identity == null ||
                //    !context.User.Identity.IsAuthenticated))
                //{
                //    context.Response.StatusCode = 401;
                //    return;
                //}

                await next();
            });

            return app;
        }
    }
}
