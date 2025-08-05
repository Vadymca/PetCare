using System;
using System.Collections.Generic;
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
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Role = table.Column<string>(type: "varchar(20)", nullable: false),
                    Preferences = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastLogin = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    ProfilePhoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Language = table.Column<string>(type: "varchar(10)", nullable: false, defaultValue: "uk"),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("CK_Users_Points", "\"Points\" >= 0");
                    table.CheckConstraint("CK_Users_Role", "\"Role\" IN ('User', 'Admin', 'Moderator')");
                });

            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breeds_Species_species_id",
                        column: x => x.species_id,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shelters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Slug = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Coordinates = table.Column<Point>(type: "geometry (point, 4326)", nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    CurrentOccupancy = table.Column<int>(type: "integer", nullable: false),
                    Photos = table.Column<List<string>>(type: "jsonb", nullable: false),
                    VirtualTourUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    WorkingHours = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SocialMedia = table.Column<Dictionary<string, string>>(type: "jsonb", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Slug = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BreedId = table.Column<Guid>(type: "uuid", nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "varchar(10)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    HealthStatus = table.Column<string>(type: "text", nullable: true),
                    Photos = table.Column<List<string>>(type: "jsonb", nullable: false),
                    Videos = table.Column<List<string>>(type: "jsonb", nullable: false),
                    ShelterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", nullable: false),
                    AdoptionRequirements = table.Column<string>(type: "text", nullable: true),
                    MicrochipId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IdNumber = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "float", nullable: true),
                    Height = table.Column<float>(type: "float", nullable: true),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsSterilized = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    HaveDocuments = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.CheckConstraint("CK_Animals_Gender", "\"Gender\" IN ('Male', 'Female', 'Unknown')");
                    table.CheckConstraint("CK_Animals_Height", "\"Height\" > 0");
                    table.CheckConstraint("CK_Animals_Status", "\"Status\" IN ('Available', 'Adopted', 'Reserved', 'InTreatment')");
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
                        name: "FK_Animals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

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
                name: "IX_Animals_Slug",
                table: "Animals",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_UserId",
                table: "Animals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_Name_species_id",
                table: "Breeds",
                columns: new[] { "Name", "species_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_species_id",
                table: "Breeds",
                column: "species_id");

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
                name: "IX_Species_Name",
                table: "Species",
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

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_shelter_occupancy()
                RETURNS TRIGGER AS $$
                BEGIN
                    IF TG_OP = 'UPDATE' AND OLD.""ShelterId"" IS DISTINCT FROM NEW.""ShelterId"" THEN
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" - 1 WHERE ""Id"" = OLD.""ShelterId"";
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" + 1 WHERE ""Id"" = NEW.""ShelterId"";
                        RETURN NEW;
                    END IF;

                    IF TG_OP = 'INSERT' THEN
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" + 1 WHERE ""Id"" = NEW.""ShelterId"";
                        RETURN NEW;
                    END IF;

                    IF TG_OP = 'DELETE' THEN
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" - 1 WHERE ""Id"" = OLD.""ShelterId"";
                        RETURN OLD;
                    END IF;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                DROP TRIGGER IF EXISTS animal_shelter_trigger ON ""Animals"";

                CREATE TRIGGER animal_shelter_trigger
                AFTER INSERT OR DELETE OR UPDATE OF ""ShelterId"" ON ""Animals""
                FOR EACH ROW EXECUTE FUNCTION update_shelter_occupancy();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropTable(
                name: "Shelters");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
