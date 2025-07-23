
using Microsoft.OpenApi.Models;
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
                Log.Information("������ PetCare.Api...");

                var builder = WebApplication.CreateBuilder(args);

                // ������������ Serilog 
                builder.Host.UseSerilog();

                // ��������� ������
                builder.Services.AddAuthorization();
                builder.Services.AddEndpointsApiExplorer();

                // ������ Swagger/OpenAPI � ��������� JWT
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

                // ��������� HTTP-������
                app.UseSerilogRequestLogging();

                // ³����� OpenAPI JSON (Swagger)
                app.UseSwagger(opt =>
                {
                    opt.RouteTemplate = "openapi/{documentName}.json";
                });

                // Scalar UI
                app.MapScalarApiReference(opt =>
                {
                    opt.Title = "PetCare API";
                    opt.Theme = ScalarTheme.Purple; // ����� ������� Mars, Saturn, etc.
                    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
                });

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "������� ���������� PetCare.Api");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
