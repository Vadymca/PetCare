
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetCare.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;
namespace PetCare.Api
{
    public class Program
    {
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

                //AppDbContext з Npgsql + NetTopologySuite
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        npgsql => npgsql.UseNetTopologySuite()));

                // Використання Serilog 
                builder.Host.UseSerilog();

                // Додавання сервісів
                builder.Services.AddAuthorization();
                builder.Services.AddEndpointsApiExplorer();

                // Додати Swagger/OpenAPI з підтримкою JWT
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
                        Scheme = "Bearer"
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                });

                var app = builder.Build();

                // Логування HTTP-запитів
                app.UseSerilogRequestLogging();

                // Віддача OpenAPI JSON (Swagger)
                app.UseSwagger(opt =>
                {
                    opt.RouteTemplate = "openapi/{documentName}.json";
                });

                // Scalar UI
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "PetCare API";
                    opt.Theme = ScalarTheme.Purple; // можна вибрати Mars, Saturn, etc.
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
}
