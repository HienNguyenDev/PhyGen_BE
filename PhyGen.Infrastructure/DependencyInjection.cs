using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PhyGen.Application.Abstractions.Authentication;
using PhyGen.Application.Abstractions.Data;
using PhyGen.Application.Common.Interfaces;
using PhyGen.Application.Common.Interfaces.Repository;
using PhyGen.Application.Services.Classes;
using PhyGen.Application.Services.ExamSets;
using PhyGen.Application.Services.Firebase;
using PhyGen.Application.Services.Questions;
using PhyGen.Application.Services.Topics;
using PhyGen.Application.Services.Users;
using PhyGen.Domain.Common;
using PhyGen.Infrastructure.Authentication;
using PhyGen.Infrastructure.Database;
using PhyGen.Infrastructure.Persistence;
using PhyGen.Infrastructure.Repositories;
using PhyGen.Infrastructure.Time;
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
            .AddSingleton(configuration)
            .AddScoped(configuration)
            .AddHTTPClient(configuration)
            .AddAuth(configuration)
            .AddHTTPContextAccessor();

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
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IFirebaseServices, FirebaseServices>();
            return services;
        }

        private static IServiceCollection AddScoped(this IServiceCollection services, IConfiguration configuration)
        {
            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IExamSetRepository, ExamSetRepository>();

            //Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Authentication
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserContext, UserContext>();

            //Serivces
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITopicServices, TopicServices>();
            services.AddScoped<IClassServices, ClassServices>();
            services.AddScoped<IQuestionServices, QuestionServices>();
            services.AddScoped<IExamSetServices, ExamSetServices>();
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
            var secretKey = configuration["Authentication:SecretKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey, 
                    };
                });

            return services;
        }


        private static IServiceCollection AddHTTPContextAccessor(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
    }
}   
