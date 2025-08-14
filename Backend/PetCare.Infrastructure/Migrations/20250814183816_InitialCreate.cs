using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:adoption_status", "pending,approved,rejected")
                .Annotation("Npgsql:Enum:aid_category", "food,medical,equipment,other")
                .Annotation("Npgsql:Enum:aid_status", "open,in_progress,fulfilled,cancelled")
                .Annotation("Npgsql:Enum:animal_gender", "male,female,unknown")
                .Annotation("Npgsql:Enum:animal_status", "available,adopted,reserved,in_treatment,dead,euthanized")
                .Annotation("Npgsql:Enum:article_status", "draft,published,archived")
                .Annotation("Npgsql:Enum:audit_operation", "insert,update,delete")
                .Annotation("Npgsql:Enum:comment_status", "pending,approved,rejected")
                .Annotation("Npgsql:Enum:donation_status", "pending,completed,failed")
                .Annotation("Npgsql:Enum:event_status", "planned,ongoing,completed,cancelled")
                .Annotation("Npgsql:Enum:event_type", "adoption_day,fundraiser,webinar,volunteer_training")
                .Annotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .Annotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .Annotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .Annotation("Npgsql:Enum:user_role", "user,admin,moderator")
                .Annotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Role = table.Column<int>(type: "user_role", nullable: false),
                    Preferences = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastLogin = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    ProfilePhoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Language = table.Column<string>(type: "text", nullable: false, defaultValue: "uk"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_Users_Points", "\"Points\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SpeciesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breeds_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "article_status", nullable: false),
                    Thumbnail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Articles_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    TableName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    Operation = table.Column<int>(type: "audit_operation", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Changes = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    NotifiableEntity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NotifiableEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationTypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shelters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Slug = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Coordinates = table.Column<Point>(type: "geometry(Point, 4326)", nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    CurrentOccupancy = table.Column<int>(type: "integer", nullable: false),
                    Photos = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: false),
                    VirtualTourUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    WorkingHours = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SocialMedia = table.Column<IReadOnlyDictionary<string, string>>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelters", x => x.Id);
                    table.CheckConstraint("CK_Shelters_Capacity", "\"Capacity\" >= 0");
                    table.ForeignKey(
                        name: "FK_Shelters_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LostPets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Slug = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LastSeenLocation = table.Column<Point>(type: "geometry (Point, 4326)", nullable: true),
                    LastSeenDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Photos = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: false),
                    Status = table.Column<int>(type: "lost_pet_status", nullable: false),
                    AdminNotes = table.Column<string>(type: "text", nullable: true),
                    Reward = table.Column<decimal>(type: "numeric", nullable: true),
                    ContactAlternative = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MicrochipId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BreedId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostPets", x => x.Id);
                    table.CheckConstraint("CK_LostPets_Reward", "\"Reward\" >= 0");
                    table.ForeignKey(
                        name: "FK_LostPets_Breeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LostPets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "comment_status", nullable: false),
                    ModeratedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComments_ArticleComments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "ArticleComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleComments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleComments_Users_ModeratedById",
                        column: x => x.ModeratedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ArticleComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalAidRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<int>(type: "aid_category", nullable: false),
                    Status = table.Column<int>(type: "aid_status", nullable: false),
                    EstimatedCost = table.Column<decimal>(type: "numeric", nullable: true),
                    Photos = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAidRequests", x => x.Id);
                    table.CheckConstraint("CK_Aid_EstimatedCost", "\"EstimatedCost\" >= 0");
                    table.ForeignKey(
                        name: "FK_AnimalAidRequests_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AnimalAidRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Slug = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<int>(type: "animal_gender", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    HealthStatus = table.Column<string>(type: "text", nullable: true),
                    Photos = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: true),
                    Videos = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: true),
                    Status = table.Column<int>(type: "animal_status", nullable: false),
                    AdoptionRequirements = table.Column<string>(type: "text", nullable: true),
                    MicrochipId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IdNumber = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: true),
                    Height = table.Column<float>(type: "real", nullable: true),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsSterilized = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    HaveDocuments = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    BreedId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShelterId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.CheckConstraint("CK_Animals_Height", "\"Height\" > 0");
                    table.CheckConstraint("CK_Animals_Weight", "\"Weight\" > 0");
                    table.ForeignKey(
                        name: "FK_Animals_Breeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animals_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animals_Shelters_ShelterId1",
                        column: x => x.ShelterId1,
                        principalTable: "Shelters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Animals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "donation_status", nullable: false),
                    TransactionId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Purpose = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Recurring = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Anonymous = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DonationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Report = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentMethodId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.CheckConstraint("CK_Donations_Amount", "\"Amount\" > 0");
                    table.ForeignKey(
                        name: "FK_Donations_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donations_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Donations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EventDate = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    Location = table.Column<Point>(type: "geometry(Point, 4326)", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "event_type", nullable: false),
                    Status = table.Column<int>(type: "event_status", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Events_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IoTDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Type = table.Column<int>(type: "io_t_device_type", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "io_t_device_status", nullable: false),
                    Data = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AlertThresholds = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IoTDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IoTDevices_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShelterSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelterSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShelterSubscriptions_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShelterSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    RequiredVolunteers = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "volunteer_task_status", nullable: false),
                    PointsReward = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<Point>(type: "geometry(Point, 4326)", nullable: true),
                    SkillsRequired = table.Column<IReadOnlyDictionary<string, string>>(type: "jsonb", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerTasks", x => x.Id);
                    table.CheckConstraint("CK_VolunteerTasks_Duration", "\"Duration\" > 0");
                    table.CheckConstraint("CK_VolunteerTasks_Points", "\"PointsReward\" >= 0");
                    table.CheckConstraint("CK_VolunteerTasks_Required", "\"RequiredVolunteers\" > 0");
                    table.ForeignKey(
                        name: "FK_VolunteerTasks_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LikedEntity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LikedEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ArticleCommentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.CheckConstraint("CK_Likes_LikedEntity", "length(\"LikedEntity\") > 0");
                    table.CheckConstraint("CK_Likes_LikedEntityId", "\"LikedEntityId\" IS NOT NULL");
                    table.CheckConstraint("CK_Likes_UserId", "\"UserId\" IS NOT NULL");
                    table.ForeignKey(
                        name: "FK_Likes_ArticleComments_ArticleCommentId",
                        column: x => x.ArticleCommentId,
                        principalTable: "ArticleComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdoptionApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Status = table.Column<int>(type: "adoption_status", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    AdminNotes = table.Column<string>(type: "text", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdoptionApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdoptionApplications_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdoptionApplications_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AdoptionApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscribedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalSubscriptions_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalTags",
                columns: table => new
                {
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTags", x => new { x.AnimalId, x.TagId });
                    table.ForeignKey(
                        name: "FK_AnimalTags_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuccessStories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Photos = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: false),
                    Videos = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Views = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuccessStories", x => x.Id);
                    table.CheckConstraint("CK_SuccessStories_Views", "\"Views\" >= 0");
                    table.ForeignKey(
                        name: "FK_SuccessStories_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuccessStories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AnimalAidDonations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DonationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalAidRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAidDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalAidDonations_AnimalAidRequests_AnimalAidRequestId",
                        column: x => x.AnimalAidRequestId,
                        principalTable: "AnimalAidRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalAidDonations_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventParticipants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamificationRewards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Used = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AwardedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamificationRewards", x => x.Id);
                    table.CheckConstraint("CK_GamificationRewards_Points", "\"Points\" >= 0");
                    table.ForeignKey(
                        name: "FK_GamificationRewards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamificationRewards_VolunteerTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "VolunteerTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerTaskAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VolunteerTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerTaskAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerTaskAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerTaskAssignments_VolunteerTasks_VolunteerTaskId",
                        column: x => x.VolunteerTaskId,
                        principalTable: "VolunteerTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_AnimalId",
                table: "AdoptionApplications",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_ApplicationDate",
                table: "AdoptionApplications",
                column: "ApplicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_ApprovedBy",
                table: "AdoptionApplications",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_Status",
                table: "AdoptionApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_UserId",
                table: "AdoptionApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidDonations_AnimalAidRequestId",
                table: "AnimalAidDonations",
                column: "AnimalAidRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidDonations_DonationId",
                table: "AnimalAidDonations",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidRequests_Category",
                table: "AnimalAidRequests",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidRequests_CreatedAt",
                table: "AnimalAidRequests",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidRequests_ShelterId",
                table: "AnimalAidRequests",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidRequests_Status",
                table: "AnimalAidRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidRequests_UserId",
                table: "AnimalAidRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_BreedId",
                table: "Animals",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_MicrochipId",
                table: "Animals",
                column: "MicrochipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ShelterId",
                table: "Animals",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ShelterId_IdNumber",
                table: "Animals",
                columns: new[] { "ShelterId", "IdNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ShelterId1",
                table: "Animals",
                column: "ShelterId1");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_Slug",
                table: "Animals",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_UserId",
                table: "Animals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSubscriptions_AnimalId",
                table: "AnimalSubscriptions",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalSubscriptions_UserId",
                table: "AnimalSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTags_TagId",
                table: "AnimalTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_ArticleId",
                table: "ArticleComments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_CreatedAt",
                table: "ArticleComments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_ModeratedById",
                table: "ArticleComments",
                column: "ModeratedById");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_ParentCommentId",
                table: "ArticleComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_Status",
                table: "ArticleComments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_UserId",
                table: "ArticleComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PublishedAt",
                table: "Articles",
                column: "PublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Status",
                table: "Articles",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_RecordId",
                table: "AuditLogs",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TableName",
                table: "AuditLogs",
                column: "TableName");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TableName_RecordId",
                table: "AuditLogs",
                columns: new[] { "TableName", "RecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_Name_SpeciesId",
                table: "Breeds",
                columns: new[] { "Name", "SpeciesId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_SpeciesId",
                table: "Breeds",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonationDate",
                table: "Donations",
                column: "DonationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_PaymentMethodId",
                table: "Donations",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_ShelterId",
                table: "Donations",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_Status",
                table: "Donations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_UserId",
                table: "Donations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_EventId",
                table: "EventParticipants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_UserId",
                table: "EventParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventDate",
                table: "Events",
                column: "EventDate");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ShelterId",
                table: "Events",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Status",
                table: "Events",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Type",
                table: "Events",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GamificationRewards_TaskId",
                table: "GamificationRewards",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_GamificationRewards_UserId",
                table: "GamificationRewards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IoTDevices_SerialNumber",
                table: "IoTDevices",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IoTDevices_ShelterId",
                table: "IoTDevices",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_IoTDevices_Status",
                table: "IoTDevices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_IoTDevices_Type",
                table: "IoTDevices",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArticleCommentId",
                table: "Likes",
                column: "ArticleCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId_LikedEntity_LikedEntityId",
                table: "Likes",
                columns: new[] { "UserId", "LikedEntity", "LikedEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LostPets_BreedId",
                table: "LostPets",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_LostPets_LastSeenDate",
                table: "LostPets",
                column: "LastSeenDate");

            migrationBuilder.CreateIndex(
                name: "IX_LostPets_Slug",
                table: "LostPets",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LostPets_Status",
                table: "LostPets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_LostPets_UserId",
                table: "LostPets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypes_Name",
                table: "NotificationTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Name",
                table: "PaymentMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shelters_Coordinates",
                table: "Shelters",
                column: "Coordinates")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "IX_Shelters_ManagerId",
                table: "Shelters",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shelters_Slug",
                table: "Shelters",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShelterSubscriptions_ShelterId",
                table: "ShelterSubscriptions",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_ShelterSubscriptions_UserId",
                table: "ShelterSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Name",
                table: "Species",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuccessStories_AnimalId",
                table: "SuccessStories",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_SuccessStories_PublishedAt",
                table: "SuccessStories",
                column: "PublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SuccessStories_UserId",
                table: "SuccessStories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTaskAssignments_UserId",
                table: "VolunteerTaskAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTaskAssignments_VolunteerTaskId",
                table: "VolunteerTaskAssignments",
                column: "VolunteerTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTasks_Date",
                table: "VolunteerTasks",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTasks_ShelterId",
                table: "VolunteerTasks",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTasks_Status",
                table: "VolunteerTasks",
                column: "Status");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_shelter_occupancy()
                RETURNS TRIGGER AS $$
                BEGIN
                    IF TG_OP = 'UPDATE' AND OLD.shelter_id IS DISTINCT FROM NEW.shelter_id THEN
                        IF OLD.shelter_id IS NOT NULL THEN
                            UPDATE shelters SET current_occupancy = current_occupancy - 1 WHERE id = OLD.shelter_id;
                        END IF;
                        IF NEW.shelter_id IS NOT NULL THEN
                            UPDATE shelters SET current_occupancy = current_occupancy + 1 WHERE id = NEW.shelter_id;
                        END IF;
                        RETURN NEW;
                    END IF;

                    IF TG_OP = 'INSERT' AND NEW.shelter_id IS NOT NULL THEN
                        UPDATE shelters SET current_occupancy = current_occupancy + 1 WHERE id = NEW.shelter_id;
                        RETURN NEW;
                    END IF;

                    IF TG_OP = 'DELETE' AND OLD.shelter_id IS NOT NULL THEN
                        UPDATE shelters SET current_occupancy = current_occupancy - 1 WHERE id = OLD.shelter_id;
                        RETURN OLD;
                    END IF;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM pg_trigger WHERE tgname = 'animal_shelter_trigger'
                    ) THEN
                        CREATE TRIGGER animal_shelter_trigger
                        AFTER INSERT OR DELETE OR UPDATE OF shelter_id
                        ON animals
                        FOR EACH ROW
                        EXECUTE FUNCTION update_shelter_occupancy();
                    END IF;
                END$$;
                ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION audit_trigger_function()
                RETURNS TRIGGER AS $$
                BEGIN
                    IF (TG_OP = 'INSERT') THEN
                        INSERT INTO ""AuditLogs"" (""TableName"", ""RecordId"", ""Operation"", ""UserId"", ""Changes"", ""CreatedAt"")
                        VALUES (TG_TABLE_NAME, NEW.id, 'Insert', NULL, row_to_json(NEW)::jsonb, CURRENT_TIMESTAMP);
                        RETURN NEW;
                    ELSIF (TG_OP = 'UPDATE') THEN
                        INSERT INTO ""AuditLogs"" (""TableName"", ""RecordId"", ""Operation"", ""UserId"", ""Changes"", ""CreatedAt"")
                        VALUES (TG_TABLE_NAME, NEW.id, 'Update', NULL, jsonb_build_object('old', row_to_json(OLD), 'new', row_to_json(NEW)), CURRENT_TIMESTAMP);
                        RETURN NEW;
                    ELSIF (TG_OP = 'DELETE') THEN
                        INSERT INTO ""AuditLogs"" (""TableName"", ""RecordId"", ""Operation"", ""UserId"", ""Changes"", ""CreatedAt"")
                        VALUES (TG_TABLE_NAME, OLD.id, 'Delete', NULL, row_to_json(OLD)::jsonb, CURRENT_TIMESTAMP);
                        RETURN OLD;
                    END IF;
                END;
                $$ LANGUAGE plpgsql;
                ");

            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    tbl RECORD;
                BEGIN
                    FOR tbl IN
                        SELECT tablename
                        FROM pg_tables
                        WHERE schemaname = 'public'
                          AND tablename <> 'AuditLogs'
                    LOOP
                        EXECUTE format($fmt$
                            DO $$
                            BEGIN
                                IF NOT EXISTS (
                                    SELECT 1
                                    FROM pg_trigger
                                    WHERE tgname = 'audit_trigger_%I'
                                ) THEN
                                    CREATE TRIGGER audit_trigger_%I
                                    AFTER INSERT OR UPDATE OR DELETE ON %I
                                    FOR EACH ROW
                                    EXECUTE FUNCTION audit_trigger_function();
                                END IF;
                            END$$;
                        $fmt$, tbl.tablename, tbl.tablename, tbl.tablename);
                    END LOOP;
                END$$;
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdoptionApplications");

            migrationBuilder.DropTable(
                name: "AnimalAidDonations");

            migrationBuilder.DropTable(
                name: "AnimalSubscriptions");

            migrationBuilder.DropTable(
                name: "AnimalTags");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "EventParticipants");

            migrationBuilder.DropTable(
                name: "GamificationRewards");

            migrationBuilder.DropTable(
                name: "IoTDevices");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "LostPets");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ShelterSubscriptions");

            migrationBuilder.DropTable(
                name: "SuccessStories");

            migrationBuilder.DropTable(
                name: "VolunteerTaskAssignments");

            migrationBuilder.DropTable(
                name: "AnimalAidRequests");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ArticleComments");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "VolunteerTasks");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropTable(
                name: "Shelters");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
