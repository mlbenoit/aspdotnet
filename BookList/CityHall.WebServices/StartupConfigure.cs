using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace CityHall.WebServices
{
    public static class StartupConfigure
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            UseForwardedHeaders(app);

            AddLogClientIpMiddleware(app);

            SetupExceptionHandler(app, env);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CityHall API");
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain",
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
                    ctx.Context.Response.Headers["Expires"] = DateTime.UtcNow.AddHours(12).ToString("R");
                }
            });
            app.UseCookiePolicy();

            app.UseStatusCodePages(async context =>
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;
                var path = request.Path.Value ?? "";

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    bool isAdmin = path.StartsWith("/admin/", StringComparison.InvariantCultureIgnoreCase);
                    if (isAdmin)
                    {
                        response.Redirect("/admin/Admins/Login");
                    }
                }
            });

            if (!Program.SkipSecurityChecks)
            {
                app.UseHttpsRedirection(); // TODO: Security - Ali
            }

            SetupHomePageRewrites(app);

            app.UseRouting();

            // ===== Use Authentication ======
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints((endpoints) =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SetupHomePageRewrites(IApplicationBuilder app)
        {
            RewriteOptions options = new RewriteOptions();
            options.AddRedirect("^$", "/admin/Home");
            options.AddRedirect("^/$", "/admin/Home");
            app.UseRewriter(options);
        }

        public static void UseForwardedHeaders(IApplicationBuilder app)
        {
            // See: https://stackoverflow.com/questions/43749236/net-core-x-forwarded-proto-not-working
            ForwardedHeadersOptions forwardingOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All, //ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
                                                         //ForwardLimit = 10,
                RequireHeaderSymmetry = false,
            };
            forwardingOptions.KnownNetworks.Clear(); //its loopback by default
            forwardingOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardingOptions);
        }

        public static void AddLogClientIpMiddleware(IApplicationBuilder app)
        {
            // middleware for logging client ip
            app.Use(async (context, next) =>
            {
                // string xForwardedForHeader = context.Request.Headers["X-Forwarded-For"].ToString();
                // Console.WriteLine($"X-Forwarded-For: {xForwardedForHeader}");
                string requestInfo = $"[{context.Connection.RemoteIpAddress}] | {context.Request.Method} | {context.Request.Path}";
                if (context.Request.QueryString != null)
                {
                    requestInfo += $"{context.Request.QueryString.Value}";
                }
                await next.Invoke();
                string responseInfo = $"Response Status: {context.Response.StatusCode}";
                string message = $"Request: {requestInfo} - {responseInfo}";
                int statusCode = context.Response.StatusCode;
                if (statusCode < 300)
                {
                    //_logger.Info(message);
                }
                else
                {
                    //_logger.Error(message);
                }
            });
        }

        public static void SetupExceptionHandler(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (true || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                return;
            }

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts(); // TODO: Ali

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    if (context.Features[typeof(IExceptionHandlerFeature)] is IExceptionHandlerFeature error)
                    {
                        await HandleError(context, error);
                    }
                    else
                    {
                        // when no error, do next.
                        await next.Invoke();
                    }
                });
            });
        }

        private static async Task HandleError(HttpContext context, IExceptionHandlerFeature error)
        {
            if (error.Error is SecurityTokenExpiredException)
            {
                // when authorization has failed, return a json message to client
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    State = "Unauthorized",
                    Msg = "Unauthorized"
                }));
            }
            else if (error.Error != null)
            {
                // when other error, return a error message json to client
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                string message = $"Error serving request. {context.Request.Path} | Exception: {error.Error}";
                //_logger.Error(message);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    State = "Internal Server Error",
                    Msg = "Internal Server Error"
                }));
            }
            else
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                string message = $"Error serving request. {context.Request.Path} | Exception: Unknown";
                //_logger.Error(message);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    State = "Internal Server Error",
                    Msg = "Unknown"
                }));
            }
        }
    }
}