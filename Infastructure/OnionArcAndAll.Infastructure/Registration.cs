using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnionArcAndAll.Application.Interfaces.RedisCache;
using OnionArcAndAll.Application.Interfaces.Tokens;
using OnionArcAndAll.Infastructure.RedisCache;
using OnionArcAndAll.Infastructure.Tokens;
using System.Text;

namespace OnionArcAndAll.Infastructure
{
    public static class Registration
    {
        public static void AddInfastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenSettings>(configuration.GetSection("JWT"));
             services.AddTransient<ITokenService, TokenService>();
           
            services.Configure<RedisCacheSettings>(configuration.GetSection("RedisCacheSettings"));
             services.AddTransient<IRedisCacheService, RedisCacheService>();
            

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };

            });


            services.AddStackExchangeRedisCache(
                opt=>
                {
                    opt.Configuration = configuration.GetConnectionString("RedisCacheSettings:ConnectionString");
                    opt.InstanceName = configuration.GetConnectionString("RedisCacheSettings:InstanceName");
                }
                );



        }
    }
}
