using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MediatR;
using EmployeeApi.Application.Employees.Commands;
using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Infrastructure.Persistence;
using EmployeeApi.Web.Services;
using EmployeeApi.Web.Controllers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JwtAuthDemo.Infrastructure;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Http;

namespace EmployeeApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks().AddDbContextCheck<EmployeeDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
            services.AddHostedService<JwtRefreshTokenCache>();
            services.AddScoped<IUserService, UserService>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Aveva Demo", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case! This really took some time for me to figure out!
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });


            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>())
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddSingleton<ICurrentUserService, DummyCurrentUserService>();
            services.AddSingleton<ICurrentUserService, ClaimsCurrentUserService>();


            if (Configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<EmployeeDbContext>(options =>
                    options.UseInMemoryDatabase("Aveva"));
            }
            else
            {
                services.AddDbContext<EmployeeDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName)));
            }

            services.AddTransient<IEmployeeDbContext, EmployeeDbContext>();
            services.AddMediatR(typeof(EmployeeController));

          

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeeApi v1"));
            }
            else
            {
                app.UseExceptionHandler();
            }
          

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(x => x.MapControllers());
        }

        // https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.0#jsonpatch-addnewtonsoftjson-and-systemtextjson
        //private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        //{
        //    var builder = new ServiceCollection()
        //        .AddLogging()
        //        .AddMvc()
        //        .AddNewtonsoftJson()
        //        .Services.BuildServiceProvider();

        //    return builder
        //        .GetRequiredService<IOptions<MvcOptions>>()
        //        .Value
        //        .InputFormatters
        //        .OfType<NewtonsoftJsonPatchInputFormatter>()
        //        .First();
        //}
    }
}















//using FluentValidation.AspNetCore;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;
//using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;
//using MediatR;
//using EmployeeApi.Application.Employees.Commands;
//using EmployeeApi.Application.Common.Interfaces;
//using EmployeeApi.Infrastructure.Persistence;
//using EmployeeApi.Web.Services;
//using EmployeeApi.Web.Controllers;
//using Microsoft.OpenApi.Models;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.Net.Http.Headers;
//using WebApi.Helpers;
//using WebApi.Services;

//namespace EmployeeApi
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }


//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
//        public void ConfigureServices(IServiceCollection services)
//        {
//            //services.AddMvc(); //change to mvccore later
//            //services.AddControllers();
//            services.AddHealthChecks()
//               .AddDbContextCheck<EmployeeDbContext>();
//            //services.AddControllers().AddXmlSerializerFormatters();
//            //// for backwards compability uppercase use this
//            //services.AddControllers().AddNewtonsoftJson(options =>
//            //{

//            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
//            // });


//            //services.AddSwaggerGen(c =>
//            //{
//            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeApi", Version = "v1" });
//            //    c.AddSecurityDefinition("token", new OpenApiSecurityScheme
//            //    {
//            //        Type = SecuritySchemeType.ApiKey,
//            //        In = ParameterLocation.Header,
//            //        Name = HeaderNames.Authorization,
//            //        Scheme = "Bearer"
//            //    });
//            //});

//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeApi", Version = "v1.0.0" });

//                var securitySchema = new OpenApiSecurityScheme
//                {
//                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
//                    Name = "Authorization",
//                    In = ParameterLocation.Header,
//                    Type = SecuritySchemeType.Http,
//                    Scheme = "bearer",
//                    Reference = new OpenApiReference
//                    {
//                        Type = ReferenceType.SecurityScheme,
//                        Id = "Bearer"
//                    }
//                };

//                c.AddSecurityDefinition("Bearer", securitySchema);

//                var securityRequirement = new OpenApiSecurityRequirement
//                {
//                    { securitySchema, new[] { "Bearer" } }
//                };

//                c.AddSecurityRequirement(securityRequirement);

//            });


//            services.AddControllers(options =>
//            {
//                options.RespectBrowserAcceptHeader = true; // false by default
//            })
//            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>())
//            .AddNewtonsoftJson()
//            .AddXmlDataContractSerializerFormatters();




//            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//            //services.AddSingleton<ICurrentUserService, DummyCurrentUserService>();
//            services.AddSingleton<ICurrentUserService, ClaimsCurrentUserService>();


//            if (Configuration.GetValue<bool>("UseInMemoryDatabase"))
//            {
//                services.AddDbContext<EmployeeDbContext>(options =>
//                    options.UseInMemoryDatabase("Aveva"));
//            }
//            else
//            {
//                services.AddDbContext<EmployeeDbContext>(options =>
//                    options.UseSqlServer(
//                        Configuration.GetConnectionString("DefaultConnection"),
//                        b => b.MigrationsAssembly(typeof(EmployeeDbContext).Assembly.FullName)));
//            }

//            services.AddTransient<IEmployeeDbContext, EmployeeDbContext>();
//            services.AddMediatR(typeof(EmployeeController));

//            // configure strongly typed settings object
//            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

//            // configure DI for application services
//            services.AddScoped<IUserService, UserService>();

//            //services.AddTransient<IUserRepository, UserRepository>();
//            //services.AddTransient<ITokenService, TokenService>();

//            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//            //{
//            //    options.TokenValidationParameters = new TokenValidationParameters
//            //    {
//            //        ValidateIssuer = true,
//            //        ValidateAudience = true,
//            //        ValidateLifetime = true,
//            //        ValidateIssuerSigningKey = true,
//            //        ValidIssuer = Configuration["Jwt:Issuer"],
//            //        ValidAudience = Configuration["Jwt:Issuer"],
//            //        IssuerSigningKey = new
//            //        SymmetricSecurityKey
//            //        (Encoding.UTF8.GetBytes
//            //        (Configuration["Jwt:Key"]))
//            //    };
//            //});



//            // custom jwt auth middleware




//            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//            //    .AddCookie(options => { options.LoginPath = "/v1/api/testdatabase/signin2"; });



//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//                app.UseSwagger();
//                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmployeeApi v1"));
//            }
//
//
//
//
//
// else
//            {
//                app.UseExceptionHandler();
//            }
//            ////app.Use401ForUnauthorizedCalls();
//            ////app.UseStaticFiles();
//            //app.UseRouting();
//            //app.UseCors();
//            //app.UseStatusCodePages();

//            //app.UseMiddleware<JwtMiddleware>();

//            //app.UseAuthorization();
//            //app.UseEndpoints(endpoints =>
//            //{
//            //    endpoints.MapControllers();
//            //});

//            app.UseRouting();

//            // global cors policy
//            app.UseCors(x => x
//                .AllowAnyOrigin()
//                .AllowAnyMethod()
//                .AllowAnyHeader());

//            // custom jwt auth middleware
//            app.UseMiddleware<JwtMiddleware>();
//            app.UseAuthentication();
//            app.UseEndpoints(x => x.MapControllers());
//        }

//        // https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.0#jsonpatch-addnewtonsoftjson-and-systemtextjson
//        //private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
//        //{
//        //    var builder = new ServiceCollection()
//        //        .AddLogging()
//        //        .AddMvc()
//        //        .AddNewtonsoftJson()
//        //        .Services.BuildServiceProvider();

//        //    return builder
//        //        .GetRequiredService<IOptions<MvcOptions>>()
//        //        .Value
//        //        .InputFormatters
//        //        .OfType<NewtonsoftJsonPatchInputFormatter>()
//        //        .First();
//        //}
//    }
//}
