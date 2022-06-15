using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PeliculasApi.Extensions
{
    public static class ServiceExtensions
    {        
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder => {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    //.AllowCredentials()
                    //builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });

            });
        }
        
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {    
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
        }

        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
