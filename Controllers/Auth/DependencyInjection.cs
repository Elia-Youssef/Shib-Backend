using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations;
using Microsoft.Extensions.Options;
using NodaTime;
using ShibAPI.Controllers.Auth.Services;
using ShibAPI.Controllers.Dto;
using System.Text;

namespace ShibAPI.Controllers.Auth
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();

            return services;
        }
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, jwtSettings);

            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            //services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters { 
            //    ValidateIssuer = true,
            //    ValidateAudience = true,
            //    ValidateLifetime = true,
            //    ValidateIssuerSigningKey = true,
            //    ValidIssuer = jwtSettings.Issuer,
            //    ValidAudience = jwtSettings.Audience,
            //    IssuerSigningKey = new SymmetricSecurityKey(
            //        Encoding.UTF8.GetBytes(jwtSettings.Secret))

            //    });
            return services;
        }
    }
}
