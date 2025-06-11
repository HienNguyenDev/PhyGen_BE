using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhyGen.Application.Abstractions.Authentication;
using PhyGen.Application.Abstractions.Data;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Infrastructure.Authentication;
using PhyGen.Infrastructure.Database;
using PhyGen.Infrastructure.Persistence;
using PhyGen.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddFirebase(configuration)
            .AddScoped(configuration)
            .AddHTTPClient(configuration)
            .AddAuth(configuration);

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<ApplicationDbContext>(
                options => options
                    .UseNpgsql(connectionString, npgsqlOptions =>
                        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                    );

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            return services;
        }

        private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("Database")!);

            return services;
        }

        private static IServiceCollection AddFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("firebase.json")
            });
            return services;
        }
        private static IServiceCollection AddSingleton(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            return services;
        }

        private static IServiceCollection AddScoped(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }

        private static IServiceCollection AddHTTPClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IJwtProvider, JwtProvider>((sp, httpClient) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();

                httpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]);
            });
            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
                    {
                        jwtOptions.Authority = configuration["Authentication:ValidIssuer"];
                        jwtOptions.Audience = configuration["Authentication:Audience"];
                        jwtOptions.TokenValidationParameters.ValidIssuer = configuration["Authentication:ValidIssuer"];
                    });
            return services;
        }
    } 
}   
