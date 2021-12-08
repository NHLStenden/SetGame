
using System;
using System.Text;
using System.Text.Json;
using Backend.Models;
using Backend.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Backend
{
    public static class ServiceExtenions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlServer(
            //         Configuration.GetConnectionString("DefaultConnection")));
            // services.AddDefaultIdentity<IdentityUser>()
            //     .AddRoles<IdentityRole>()
            //     .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityCore<Player>(q =>
                {
                    q.User.RequireUniqueEmail = true;
                    q.SignIn.RequireConfirmedAccount = false;
                    q.SignIn.RequireConfirmedEmail = false;
                    q.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddRoles<IdentityRole<int>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SetContext>();
        }

        public static void ConfigureJwt(this IServiceCollection services, JwtSettings jwtSettings)
        {
            
            var optionsTokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };

            services.AddSingleton(optionsTokenValidationParameters);
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = optionsTokenValidationParameters;
            });
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder => builder.Run(async context => {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    //Todo: in a real application write error to some logfile (or other useful resource)
                    Console.WriteLine($"ContextFeature: {contextFeature.Error}");   
                        
                    var error = new Error
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error"
                    };

                    var jsonError = JsonSerializer.Serialize(error);
                    await context.Response.WriteAsync(jsonError); 
                }
            }));
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(0, 1);
            });
        }
    }
}