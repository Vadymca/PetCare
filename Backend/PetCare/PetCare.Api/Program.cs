namespace PetCare.Api;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            var envSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (!string.IsNullOrEmpty(envSecret))
            {
                builder.Configuration["Jwt:Secret"] = envSecret;
            }

            // -------------------- Authentication & Authorization --------------------
            builder.Services.AddAuthentication(options =>
            {
                // Встановлюємо JWT як схему за замовчуванням
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                                ?? builder.Configuration["JwtSettings:SecretKey"]
                                ?? throw new InvalidOperationException("JWT SecretKey не встановлено");

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            // Застосовуємо схему авторизації за замовчуванням
            builder.Services.AddAuthorization(options =>
            {
                // Якщо потрібно, можна додати політики
                options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();
            });

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
            app.MapRegisterEndpoint(); // /api/auth/register
            app.MapLoginEndpoint(); // /api/auth/login
            app.MapLogoutEndpoint(); // /api/auth/logout
            app.MapRefreshEndpoint(); // /api/auth/refresh
            app.MapForgotPasswordEndpoint(); // /api/auth/forgot-password
            app.MapResetPasswordEndpoint(); // /api/auth/reset-password
            app.MapConfirmEmailEndpoint(); // /api/auth/confirm-email
            app.MapResendVerificationEndpoint(); // /api/auth/resend-verification

            app.MapSetupTotpEndpoint(); // /api/auth/2fa/totp/setup
            app.MapVerifyTotpSetupEndpoint(); // /api/auth/2fa/totp/verify-setup
            app.MapVerifyTotpEndpoint(); // /api/auth/2fa/totp/verify
            app.MapDisableTotpEndpoint(); // /api/auth/2fa/totp/disable
            app.MapGetTotpBackupCodesEndpoint(); // /api/auth/2fa/totp/backup-codes
            app.MapRegenerateBackupCodesEndpoint(); // /api/auth/2fa/totp/regenerate-backup-codes
            app.MapVerifyTotpBackupCodeEndpoint(); // /api/auth/2fa/totp/verify-backup-code

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
