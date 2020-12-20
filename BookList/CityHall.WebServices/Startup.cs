using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookList.WebServices.Auth;
using Bookrental.Services.Identity;
using BookRental.Entities;
using BookRental.Persistence;
using BookRental.Services.Contracts.Identity;
using BookRental.Services.Identity;
using BookRental.WebServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            // make sure that all emails are unique, otherwise UserManager.FindByEmail
            // will throw.

            services.Configure<IdentityOptions>(options =>
            {
              
                options.User.RequireUniqueEmail = true;

            });

            AddIdentityService(services);

            services.AddControllersWithViews();

            SetupPryzeboxServices(services, Configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "BookRental API" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
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


            //Services

        }
    }
}
