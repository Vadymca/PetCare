namespace PetCare.Api;

using System.Threading.RateLimiting;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using PetCare.Api.Authorization;
using PetCare.Api.Endpoints.AdoptionApplications;
using PetCare.Api.Endpoints.AnimalAidRequests;
using PetCare.Api.Endpoints.Animals;
using PetCare.Api.Endpoints.Auth;
using PetCare.Api.Endpoints.Auth.Facebook;
using PetCare.Api.Endpoints.Auth.Google;
using PetCare.Api.Endpoints.Auth.TwoFactor;
using PetCare.Api.Endpoints.Auth.TwoFactor.Sms;
using PetCare.Api.Endpoints.Breeds;
using PetCare.Api.Endpoints.Media;
using PetCare.Api.Endpoints.PaymentMethods;
using PetCare.Api.Endpoints.Payments.Donations;
using PetCare.Api.Endpoints.Payments.Guardianships;
using PetCare.Api.Endpoints.Payments.History;
using PetCare.Api.Endpoints.Payments.Intents;
using PetCare.Api.Endpoints.Payments.LiqPay;
using PetCare.Api.Endpoints.Payments.Subscriptions;
using PetCare.Api.Endpoints.Shelters;
using PetCare.Api.Endpoints.Species;
using PetCare.Api.Endpoints.Users;
using PetCare.Api.Middleware;
using PetCare.Api.Swagger;
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
    /// /// <param name="args">Command-line arguments.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Obsolete]
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

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                ContentRootPath = Directory.GetCurrentDirectory(),
                Args = args,
            });

            // -------------------- Configuration Loading --------------------
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            var envSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (!string.IsNullOrEmpty(envSecret))
            {
                builder.Configuration["Jwt:Secret"] = envSecret;
            }

            // -------------------- Enable Dynamic JSON for Npgsql --------------------
            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

            // -------------------- CORS --------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PetCarePolicy", policy =>
                {
                    policy.WithOrigins(
                        "https://dobrodii.onrender.com",
                        "http://localhost:4200",
                        "https://localhost:4200",
                        "http://192.168.1.104",
                        "https://192.168.1.104")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

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

                options.TokenValidationParameters = new TokenValidationParameters
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
                // Політика для Admin
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                // Політика для власника ресурсу або Admin
                options.AddPolicy("ResourceOwnerOrAdmin", policy =>
                    policy.Requirements.Add(new ResourceOwnerOrAdminRequirement()));

                // Політика для ShelterManager або Admin
                options.AddPolicy("CanManageAnimals", policy =>
                    policy.RequireRole("Admin", "ShelterManager"));

                options.AddPolicy("CanManageShelter", policy =>
                    policy.RequireRole("Admin", "ShelterManager"));

                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();
            });

            // Реєструємо handler для перевірки власника ресурсу
            builder.Services.AddSingleton<IAuthorizationHandler, ResourceOwnerOrAdminHandler>();

            // -------------------- DbContext --------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? builder.Configuration["ConnectionStrings__DefaultConnection"] // для Docker
                    ?? throw new InvalidOperationException("Рядок підключення до бази даних не знайдено.");

                options.UseNpgsql(
                    connectionString,
                    npgsql =>
                    {
                        npgsql.MigrationsAssembly("PetCare.Infrastructure");
                        npgsql.UseNetTopologySuite();

                        // Enum mapping
                        npgsql.UseNetTopologySuite();
                        npgsql.MapEnum<AdoptionStatus>("adoption_status");
                        npgsql.MapEnum<AidCategory>("aid_category");
                        npgsql.MapEnum<AidStatus>("aid_status");
                        npgsql.MapEnum<AnimalGender>("animal_gender");
                        npgsql.MapEnum<AnimalSize>("animal_size");
                        npgsql.MapEnum<AnimalStatus>("animal_status");
                        npgsql.MapEnum<AnimalTemperament>("animal_temperament");
                        npgsql.MapEnum<ArticleStatus>("article_status");
                        npgsql.MapEnum<AuditOperation>("audit_operation");
                        npgsql.MapEnum<CommentStatus>("comment_status");
                        npgsql.MapEnum<DonationStatus>("donation_status");
                        npgsql.MapEnum<EventStatus>("event_status");
                        npgsql.MapEnum<EventType>("event_type");
                        npgsql.MapEnum<IoTDeviceStatus>("io_t_device_status");
                        npgsql.MapEnum<IoTDeviceType>("io_t_device_type");
                        npgsql.MapEnum<LostPetStatus>("lost_pet_status");
                        npgsql.MapEnum<UserRole>("user_role");
                        npgsql.MapEnum<VolunteerTaskStatus>("volunteer_task_status");
                        npgsql.MapEnum<GuardianshipStatus>("guardianship_status");
                        npgsql.MapEnum<SubscriptionScope>("subscription_scope");
                        npgsql.MapEnum<SubscriptionStatus>("subscription_status");
                        npgsql.MapEnum<PaymentIntentStatus>("payment_intent_status");
                    })
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
            });

            // -------------------- Application & Infrastructure --------------------
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            // -------------------- MediatR + FluentValidation + AutoMapper--------------------
            builder.Services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);

            builder.Services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(Application.Common.Behaviors.ValidationBehavior<,>));

            // -------------------- AutoMapper --------------------
            // Реєструємо AutoMapper з усіма профілями поточної збірки
            builder.Services.AddAutoMapper(
                cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            },
                AppDomain.CurrentDomain.GetAssemblies());

            // -------------------- Identity --------------------
            builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
             .AddRoles<AppRole>()
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

            // -------------------- HttpContextAccessor --------------------
            builder.Services.AddHttpContextAccessor();

            // -------------------- Controllers --------------------
            builder.Services.AddControllers()
             .AddJsonOptions(options =>
             {
                 // Enum -> string (camelCase у JSON)
                 options.JsonSerializerOptions.Converters.Insert(
                    0,
                    new Serialization.FlexibleStringEnumConverterFactory(
                        System.Text.Json.JsonNamingPolicy.CamelCase,
                        allowIntegerValues: true));
             });

            // -------------------- Minimal API JSON Options --------------------
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                // Flexible enum converter: дозволяє string ("available") і int (0)
                options.SerializerOptions.Converters.Insert(
                    0,
                    new Serialization.FlexibleStringEnumConverterFactory(
                        System.Text.Json.JsonNamingPolicy.CamelCase,
                        allowIntegerValues: true));
            });

            // -------------------- Logging --------------------
            builder.Host.UseSerilog();

            // -------------------- Authorization & Swagger --------------------
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PetCare API", Version = "v1" });

                opt.SchemaGeneratorOptions.SchemaFilters.Add(new EnumSchemaFilter(System.Text.Json.JsonNamingPolicy.CamelCase));

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

            // -------------------- CSRF --------------------
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN"; // Клієнт повинен відправляти цей заголовок
            });

            // -------------------- Rate Limiting --------------------
            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("GlobalPolicy", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 10,
                    }));
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }


            app.UseExceptionHandling();
            app.UseStaticFiles();
            app.UseHttpsRedirection();


            app.UseRouting();
            app.UseCors("PetCarePolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();

            //app.UseAntiforgery();

            // Явно відповідаємо на всі OPTIONS-запити (preflight)
            app.MapMethods("{*any}", new[] { "OPTIONS" }, (HttpContext context) =>
            {
                context.Response.StatusCode = 200;
                return Task.CompletedTask;
            }).RequireCors("PetCarePolicy");


            // app.MapGet("/api/csrf-token", (IAntiforgery antiforgery, HttpContext context) =>
            // {
            // var tokens = antiforgery.GetAndStoreTokens(context);
            // return Results.Ok(new { token = tokens.RequestToken });
            // });
            // app.Use(async (context, next) =>
            // {
            // var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
            // if (HttpMethods.IsPost(context.Request.Method) ||
            //  HttpMethods.IsPut(context.Request.Method) ||
            //  HttpMethods.IsDelete(context.Request.Method))
            // {
            // await antiforgery.ValidateRequestAsync(context);
            // }
            // await next();
            // });

            // -------------------- Logging & Swagger --------------------
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

            // --------------------Endpoints--------------------
            // ----------------------Auth-----------------------
            app.MapRegisterEndpoint(); // /api/auth/register
            app.MapLoginEndpoint(); // /api/auth/login
            app.MapLogoutEndpoint(); // /api/auth/logout
            app.MapRefreshEndpoint(); // /api/auth/refresh
            app.MapForgotPasswordEndpoint(); // /api/auth/forgot-password
            app.MapResetPasswordEndpoint(); // /api/auth/reset-password
            app.MapConfirmEmailEndpoint(); // /api/auth/confirm-email
            app.MapResendVerificationEndpoint(); // /api/auth/resend-verification

            // --------------------TwoFactor----------------------
            app.MapSetupTotpEndpoint(); // /api/auth/2fa/totp/setup
            app.MapVerifyTotpSetupEndpoint(); // /api/auth/2fa/totp/verify-setup
            app.MapVerifyTotpEndpoint(); // /api/auth/2fa/totp/verify
            app.MapDisableTotpEndpoint(); // /api/auth/2fa/totp/disable
            app.MapGetTotpBackupCodesEndpoint(); // /api/auth/2fa/totp/backup-codes
            app.MapRegenerateBackupCodesEndpoint(); // /api/auth/2fa/totp/regenerate-backup-codes
            app.MapVerifyTotpBackupCodeEndpoint(); // /api/auth/2fa/totp/verify-backup-code

            // --------------------TwoFactor-Sms---------------------
            app.MapSetupSms2FaEndpoint(); // /api/auth/2fa/sms/setup
            app.MapVerifySms2FaSetupEndpoint(); // /api/auth/2fa/sms/verify-setup
            app.MapSendSms2FaCodeEndpoint(); // /api/auth/2fa/sms/send
            app.MapVerifySms2FaCodeEndpoint(); // /api/auth/2fa/sms/verify
            app.MapDisableSms2FaEndpoint(); // /api/auth/2fa/sms/disable

            // ------------------TwoFactor-Management-------------------
            app.MapTwoFactorStatusEndpoint(); // /api/auth/2fa/status
            app.MapDisableAllTwoFactorEndpoint(); // /api/auth/2fa/disable-all
            app.MapRecoveryCodesEndpoint(); // /api/auth/2fa/recovery-codes
            app.MapUseRecoveryCodeEndpoint(); // /api/auth/2fa/use-recovery-code

            // --------------------Auth-Facebook----------------------
            app.MapFacebookLoginEndpoint(); // /api/auth/facebook
            app.MapFacebookCallbackEndpoint(); // /api/auth/facebook/callback

            // ---------------------Auth-Google-----------------------
            app.MapGoogleLoginEndpoint(); // /api/auth/google
            app.MapGoogleCallbackEndpoint(); // /api/auth/google/callback

            // ----------------------Media-----------------------
            app.MapUploadMediaEndpoint(); // /api/media/upload

            // ----------------------Users-----------------------
            app.MapAddUserRoleEndpoint(); // /api/users/{id}/roles
            app.MapGetUsersEndpoint(); // /api/users
            app.MapGetUserByIdEndpoint(); // /api/users/{id}
            app.MapUpdateUserEndpoint(); // /api/users/{id}
            app.MapUpdateMyProfileEndpoint(); // /api/users/me
            app.MapDeleteUserEndpoint(); // /api/users/{id}
            app.MapGetCurrentUserEndpoint(); // /api/users/me
            app.MapGetUserSubscriptionsEndpoint(); // /api/users/{id}/subscriptions
            app.MapGetUserActivityEndpoint(); // /api/users/{id}/activity

            // ----------------------Animals-----------------------
            app.MapGetAnimalsEndpoint(); // /api/animals
            app.MapGetAnimalByIdEndpoint(); // /api/animals/{id}
            app.MapGetAnimalBySlugEndpoint(); // /api/animals/{slug}
            app.MapCreateAnimalEndpoint(); // /api/animals
            app.MapUpdateAnimalEndpoint(); // /api/animals/{id}
            app.MapDeleteAnimalEndpoint(); // /api/animals/{id}
            app.MapAddAnimalPhotoEndpoint(); // /api/animals/{id}/photos
            app.MapRemoveAnimalPhotoEndpoint(); // /api/animals/{id:guid}/photos
            app.MapSubscribeToAnimalEndpoint(); // /api/animals/{id}/subscribe
            app.MapUnsubscribeFromAnimalEndpoint(); // /api/animals/{id:guid}/subscribe
            app.MapGetFavoriteAnimalsEndpoint(); // /api/animals/favorites
            app.MapGetAnimalSubscriptionsByUserIdEndpoint(); // /api/animals/subscriptions/{userId}

            // ----------------------Shelters-----------------------
            app.MapGetSheltersEndpoint(); // /api/shelters
            app.MapGetShelterByIdEndpoint(); // /api/shelters/{id}
            app.MapGetShelterBySlugEndpoint(); // /api/shelters/{slug}
            app.MapCreateShelterEndpoint(); // /api/shelters
            app.MapUpdateShelterEndpoint(); // /api/shelters/{id}
            app.MapDeleteShelterEndpoint(); // /api/shelters/{id}
            app.MapAddShelterPhotoEndpoint(); // /api/shelters/{id}/photos
            app.MapRemoveShelterPhotoEndpoint(); // /api/shelters/{id:guid}/photos
            app.MapSubscribeToShelterEndpoint(); // /api/shelters/{id}/subscribe
            app.MapUnsubscribeFromShelterEndpoint(); // /api/shelters/{id:guid}/subscribe
            app.MapGetFavoriteSheltersEndpoint(); // /api/shelters/favorites

            // --------------------- Species ---------------------
            app.MapGetSpeciesEndpoint(); // /api/species
            app.MapGetSpecieByIdEndpoint(); // /api/species/{id}
            app.MapGetBreedsEndpoint(); // /api/species/{speciesId}/breeds
            app.MapCreateSpecieEndpoint(); // /api/species
            app.MapUpdateSpecieEndpoint(); // /api/species/{id}
            app.MapDeleteSpecieEndpoint(); // /api/species/{id}

            // ---------------------- Breeds ----------------------
            app.MapGetAllBreedsEndpoint(); // /api/breeds
            app.MapGetBreedByIdEndpoint(); // /api/breeds/{id}
            app.MapGetBreedsBySpecieEndpoint(); // /api/breeds/species/{speciesId}
            app.MapCreateBreedEndpoint(); // /api/breeds
            app.MapUpdateBreedEndpoint(); // /api/breeds/{id}
            app.MapDeleteBreedEndpoint(); // /api/breeds/{id}

            // ---------------------- Payments ----------------------
            // ----------------------- LiqPay -----------------------
            app.MapLiqPayCheckoutEndpoint(); // /api/payments/liqpay/checkout
            app.MapLiqPayCallbackEndpoint(); // /api/payments/liqpay/callback
            app.MapLiqPayStatusEndpoint(); // /api/payments/liqpay/status

            // ---------------------- Donations ----------------------
            app.MapGetAllDonationsEndpoint(); // /api/payments/donations/all
            app.MapGetProjectDonationsEndpoint(); // /api/payments/donations/project/{projectId:guid}

            // -------------------- Subscriptions ---------------------
            app.MapCancelSubscriptionEndpoint(); // /api/subscriptions/{providerSubscriptionId}/cancel
            app.MapGetMySubscriptionsEndpoint(); // /api/payments/me/subscriptions
            app.MapResetSubscriptionEndpoint(); // /api/subscriptions/{subscriptionId:guid}/reset

            // -------------------- Guardianships ---------------------
            app.MapCreateGuardianshipEndpoint(); // /api/guardianships
            app.MapGetMyGuardianshipsEndpoint(); // /api/guardianships/me
            app.MapDeleteGuardianshipEndpoint(); // /api/guardianships/{id:guid}

            // ------------------- Payment History --------------------
            app.MapGetMyPaymentHistoryEndpoint(); // /api/payments/me/history
            app.MapGetMyUpcomingPaymentsEndpoint(); // /api/payments/me/upcoming

            // ----------------------- Intents ------------------------
            app.MapGetPaymentIntentEndpoint(); // /api/payments/intents/{orderId}

            // -------------------- PaymentMethods --------------------
            app.MapGetAllPaymentMethodsEndpoint(); // /api/payment-methods
            app.MapGetPaymentMethodByIdEndpoint(); // /api/payment-methods/{id:guid}
            app.MapCreatePaymentMethodEndpoint(); // /api/payment-methods
            app.MapUpdatePaymentMethodEndpoint(); // /api/payment-methods/{id:guid}
            app.MapDeletePaymentMethodEndpoint(); // /api/payment-methods/{id:guid}

            // -------------------- AnimalAidRequest --------------------
            app.MapGetAllAnimalAidRequestsEndpoint(); // /api/animal-aid-requests
            app.MapGetAnimalAidRequestByIdEndpoint(); // /api/animal-aid-requests/{id:guid}
            app.MapGetAnimalAidRequestBySlugEndpoint(); // /api/animal-aid-requests/{slug}
            app.MapCreateAnimalAidRequestEndpoint(); // /api/animal-aid-requests
            app.MapUpdateAnimalAidRequestEndpoint(); // /api/animal-aid-requests/{id:guid}
            app.MapUpdateAnimalAidRequestStatusEndpoint(); // /api/animal-aid-requests/{id:guid}/status
            app.MapDeleteAnimalAidRequestEndpoint(); // /api/animal-aid-requests/{id:guid}
            app.MapGetUrgentAnimalAidRequestsEndpoint(); // /api/animal-aid-requests/urgent

            // -------------------- AdoptionApplication --------------------
            app.MapApproveAdoptionApplicationEndpoint(); // /api/adoption-applications/{id:guid}/approve
            app.MapRejectAdoptionApplicationEndpoint(); // /api/adoption-applications/{id:guid}/reject
            app.MapChangeAdoptionApplicationStatusEndpoint(); // /api/adoption-applications/{id:guid}/change-status
            app.MapCreateAdoptionApplicationEndpoint(); // /api/adoption-applications
            app.MapDeleteAdoptionApplicationEndpoint(); // /api/adoption-applications/{id:guid}
            app.MapGetAllAdoptionApplicationsEndpoint(); // /api/adoption-applications
            app.MapGetApprovedAdoptionApplicationsEndpoint(); // /api/adoption-applications/approved
            app.MapGetAdoptionApplicationByIdEndpoint(); // /api/adoption-applications/{id:guid}
            app.MapGetMyAdoptionApplicationsEndpoint(); // /api/adoption-applications/my
            app.MapGetPendingAdoptionApplicationsEndpoint(); // /api/adoption-applications/pending
            app.MapGetRejectedAdoptionApplicationsEndpoint(); // /api/adoption-applications/rejected
            app.MapUpdateAdoptionApplicationEndpoint(); // /api/adoption-applications/{id:guid}
            app.MapCompleteAdoptionApplicationEndpoint(); // /api/adoption-applications/{id:guid}/complete

            app.MapGet("/", () => Results.Ok("✅ PetCare.Api is running successfully!"));

            // -------------------- Migrations & Seeding --------------------
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Отримуємо DbContext
                var dbContext = services.GetRequiredService<AppDbContext>();

                // Застосовуємо міграції з правильною збіркою
                await dbContext.Database.MigrateAsync(); // Міграції беруться з MigrationsAssembly, що задано у UseNpgsql

                // Виконуємо seed ролей та інших даних
                await DataSeeder.SeedAsync(services);
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
