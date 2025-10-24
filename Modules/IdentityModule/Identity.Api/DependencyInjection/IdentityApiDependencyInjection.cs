using Identity.Api.Services;
using Identity.Application.Contracts.Services;
using Identity.Application.DependencyInjection;
using Identity.Application.Settings;
using Identity.Domain.Entities;
using Identity.Infrastructure.DependencyInjection;
using Identity.Persistense.Context;
using Identity.Persistense.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.DependencyInjection
{
    public static class IdentityApiDependencyInjection 
    {
        public static IServiceCollection AddIdentityApiDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {

            #region PreLayers
            services.AddIdentityPersistenceDependencyInjection(configuration);
            services.AddIdentityInfrastructureDependencyInjection(configuration);
            services.AddIdentityApplicationDependencyInjection(configuration);
            #endregion


            services.AddScoped<IJwtTokenService, JwtTokenService>();


            #region Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;

            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
            #endregion

            #region JWT settings


            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion
            return services;
        }
    }
}
