using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookList.WebServices.Auth;
using Bookrental.Services.Identity;
using BookRental.Entities;
using BookRental.Persistence;
using BookRental.Persistence.Contracts;
using BookRental.Persistence.Persistence;
using BookRental.Services;
using BookRental.Services.Contracts;
using BookRental.Services.Contracts.Identity;
using BookRental.Services.Identity;
using BookRental.WebServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace CityHall.WebServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // set up the connection WITHOUT actually connecting to the database
            string connectionString = Configuration.GetConnectionString(WebServiceConstants.DB_CONTEXT_NAME);
            services.AddDbContext<BookDbContext>(option =>
            {
                option.UseSqlServer(connectionString, option =>
                             option.MigrationsAssembly(WebServiceConstants.MIGRATIONS_ASSEMBLY_NAME));

            });

            AddIdentityService(services);

            // ==== Add Authentication ====

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/user/Users/Login";
                    options.AccessDeniedPath = "/user/Forbidden";
                    options.SlidingExpiration = true;
                });

            //Assembly ControllerAssembly = typeof(UsersController).Assembly;

            IMvcBuilder mvcBuilder = services.AddControllersWithViews(options =>
            {
                //AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                //  .RequireAuthenticatedUser()
                //  .Build();
                if (!Program.SkipSecurityChecks)
                {
                    options.Filters.Add(new AuthorizeFilter()); // gloabal filter // TODO: SECURITY - JAGAN
                }
            });

            //x.AddJsonOptions(SetupTextJsonOptions);
            mvcBuilder.AddNewtonsoftJson(SetupNewtonsoftJsonOptions);
            mvcBuilder.AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);
#if DEBUG
            mvcBuilder.AddRazorRuntimeCompilation();
#endif


            // make sure that all emails are unique, otherwise UserManager.FindByEmail
            // will throw.

            services.Configure<IdentityOptions>(options =>
            {
              
                options.User.RequireUniqueEmail = true;

            });

     

            SetupPryzeboxServices(services, Configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "BookRental API" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StartupConfigure.Configure(app, env, Configuration);
            SeedDatabase(app);
        }

        private static void SeedDatabase(IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                BookDbContext context = serviceScope.ServiceProvider.GetService<BookDbContext>();
                //context.Database.Migrate();
                context.EnsureSeedData().GetAwaiter().GetResult();
            }
        }

        public void AddIdentityService(IServiceCollection services)
        {
            services.AddIdentity<UserEntity, UserRoleEntity>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 2;
            })
                .AddEntityFrameworkStores<BookDbContext>()
                .AddDefaultTokenProviders();
        }

        public void SetupPryzeboxServices(IServiceCollection services, IConfiguration configuration)
        {
            //AspNet Identity, Authentication, & Authorization services

            services.AddScoped<IAuthorizationHandler, UserIsOwnerAuthorizationHandler>();
            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            services.AddScoped<ISignInManager, SignInManagerWrapper>();
            services.AddScoped<IUserManager, UserManagerWrapper>();

            // DB Repositories
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();

            //Services
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUsersAccountsService, UsersAccountsService>();
        }

        private static void SetupNewtonsoftJsonOptions(MvcNewtonsoftJsonOptions obj)
        {
            obj.SerializerSettings.ContractResolver = new DefaultContractResolver();
            obj.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }
    }
}
