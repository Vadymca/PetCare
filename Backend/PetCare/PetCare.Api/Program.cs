namespace PetCare.Api;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using PetCare.Api.Endpoints.Auth;
using PetCare.Api.Endpoints.Auth.TwoFactor;
using PetCare.Application;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using PetCare.Infrastructure;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Identity;
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
    public static async Task Main(string[] args)
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

            // -------------------- DbContext --------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsql =>
                    {
                        npgsql.UseNetTopologySuite();
                        npgsql.MapEnum<UserRole>("user_role");
                    })
                       .EnableSensitiveDataLogging()
                       .EnableDetailedErrors());

            // -------------------- Application & Infrastructure --------------------
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            // -------------------- MediatR + FluentValidation --------------------
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);
            });

            builder.Services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);

            builder.Services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(Application.Common.Behaviors.ValidationBehavior<,>));

            // -------------------- Identity --------------------
            builder.Services.AddIdentity<User, AppRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // -------------------- HttpContextAccessor --------------------
            builder.Services.AddHttpContextAccessor();

            // -------------------- Controllers --------------------
            builder.Services.AddControllers();

            // -------------------- Logging --------------------
            builder.Host.UseSerilog();

            // -------------------- Authorization & Swagger --------------------
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

            // --------------------Endpoints--------------------
            app.MapRegisterEndpoint();
            app.MapLoginEndpoint();
            app.MapLogoutEndpoint();
            app.MapRefreshEndpoint();
            app.MapForgotPasswordEndpoint();
            app.MapResetPasswordEndpoint();
            app.MapConfirmEmailEndpoint();
            app.MapResendVerificationEndpoint();

            app.MapSetupTotpEndpoint();

            // -------------------- Middleware --------------------
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

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature?.Error != null)
                    {
                        var ex = exceptionHandlerPathFeature.Error;

                        switch (ex)
                        {
                            case FluentValidation.ValidationException validationEx:
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                var validationResult = new
                                {
                                    errors = validationEx.Errors.Select(e => e.ErrorMessage).ToArray(),
                                };
                                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(validationResult));
                                break;

                            case InvalidOperationException:
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                                {
                                    error = ex.Message,
                                }));
                                break;

                            default:
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                                {
                                    error = ex.Message,
                                }));
                                break;
                        }
                    }
                });
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // -------------------- Migrations & Seeding --------------------
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();

                await DataSeeder.SeedAsync(scope.ServiceProvider);
            }

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
