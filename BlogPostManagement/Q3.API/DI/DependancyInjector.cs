using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Q3.Business.BusinessServices;
using Q3.Business.IBusinessServices;
using Q3.Data;
using Q3.Data.IRepository;
using Q3.Data.Repository;
using Q3.Integration.Services;
using Q3.Shared.Validators.MainData;
using System.Text;
using Q3.AutoMapper;
using Q3.Shared.Interfaces;

namespace Q3.API.DI
{
    public static class DependancyInjector
    {
        public static void RegisterServices(IServiceCollection services, ConfigurationManager configuration)
        {

            // Register DbContext
            services.AddDbContext<BlogDbContext>(options =>
     options.UseSqlServer(configuration.GetConnectionString("BlogPostManagementDb"),
         b => b.MigrationsAssembly("Q3.Data")));

            // AutoMapper
            services.AddAutoMapper(typeof(BlogPostProfile).Assembly);


            // Business & Repository Services
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Validators
            services.AddValidatorsFromAssemblyContaining<BlogPostDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();

            // Token service
            services.AddScoped<ITokenService, TokenService>();

            // JWT Authentication
            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key configuration is missing.");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog Post Management API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT token (e.g., 'eyJ...')"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

        }

    }
}
