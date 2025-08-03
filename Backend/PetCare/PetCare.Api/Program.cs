// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PetCare.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetCare.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;

/// <summary>
/// The main entry point class for the PetCare API application.
/// </summary>
public class Program
{
    /// <summary>
    /// Application entry point.
    /// Configures services, middleware, and runs the web application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        try
        {
            Log.Information("Запуск PetCare.Api...");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsql => npgsql.UseNetTopologySuite()));

            builder.Host.UseSerilog();

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PetCare API", Version = "v1" });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer eyJhbGci...')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme,
                        },
                    },
                    Array.Empty<string>()
                },
            });
            });

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.UseSwagger(opt =>
            {
                opt.RouteTemplate = "openapi/{documentName}.json";
            });

            app.MapScalarApiReference(opt =>
            {
                opt.Title = "PetCare API";
                opt.Theme = ScalarTheme.Mars;
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Аварійне завершення PetCare.Api");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
