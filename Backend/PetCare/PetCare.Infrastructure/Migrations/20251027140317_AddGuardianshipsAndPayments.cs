using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PetCare.Domain.Enums;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGuardianshipsAndPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:adoption_status", "pending,approved,rejected")
                .Annotation("Npgsql:Enum:aid_category", "food,medical,equipment,other")
                .Annotation("Npgsql:Enum:aid_status", "open,in_progress,fulfilled,cancelled")
                .Annotation("Npgsql:Enum:animal_gender", "male,female,unknown")
                .Annotation("Npgsql:Enum:animal_size", "small,medium,medium_plus,large")
                .Annotation("Npgsql:Enum:animal_status", "available,adopted,reserved,in_treatment,dead,euthanized")
                .Annotation("Npgsql:Enum:animal_temperament", "friendly,shy,needs_socialization,independent,affectionate,protective,curious,playful,calm,energetic,gentle,vocal,quiet,cuddly,nervous,confident,food_motivated,trainable,stubborn,good_with_kids,good_with_other_animals,needs_experienced_owner,senior_and_relaxed,young_and_learning,special_needs,bonded_pair")
                .Annotation("Npgsql:Enum:article_status", "draft,published,archived")
                .Annotation("Npgsql:Enum:audit_operation", "insert,update,delete")
                .Annotation("Npgsql:Enum:comment_status", "pending,approved,rejected")
                .Annotation("Npgsql:Enum:donation_status", "pending,completed,failed")
                .Annotation("Npgsql:Enum:event_status", "planned,ongoing,completed,cancelled")
                .Annotation("Npgsql:Enum:event_type", "adoption_day,fundraiser,webinar,volunteer_training")
                .Annotation("Npgsql:Enum:guardianship_status", "requires_payment,active,completed")
                .Annotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .Annotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .Annotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .Annotation("Npgsql:Enum:subscription_scope", "global,aid_request,guardianship")
                .Annotation("Npgsql:Enum:subscription_status", "active,canceled,paused")
                .Annotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .Annotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:Enum:adoption_status", "pending,approved,rejected")
                .OldAnnotation("Npgsql:Enum:aid_category", "food,medical,equipment,other")
                .OldAnnotation("Npgsql:Enum:aid_status", "open,in_progress,fulfilled,cancelled")
                .OldAnnotation("Npgsql:Enum:animal_gender", "male,female,unknown")
                .OldAnnotation("Npgsql:Enum:animal_size", "small,medium,medium_plus,large")
                .OldAnnotation("Npgsql:Enum:animal_status", "available,adopted,reserved,in_treatment,dead,euthanized")
                .OldAnnotation("Npgsql:Enum:animal_temperament", "friendly,shy,needs_socialization,independent,affectionate,protective,curious,playful,calm,energetic,gentle,vocal,quiet,cuddly,nervous,confident,food_motivated,trainable,stubborn,good_with_kids,good_with_other_animals,needs_experienced_owner,senior_and_relaxed,young_and_learning,special_needs,bonded_pair")
                .OldAnnotation("Npgsql:Enum:article_status", "draft,published,archived")
                .OldAnnotation("Npgsql:Enum:audit_operation", "insert,update,delete")
                .OldAnnotation("Npgsql:Enum:comment_status", "pending,approved,rejected")
                .OldAnnotation("Npgsql:Enum:donation_status", "pending,completed,failed")
                .OldAnnotation("Npgsql:Enum:event_status", "planned,ongoing,completed,cancelled")
                .OldAnnotation("Npgsql:Enum:event_type", "adoption_day,fundraiser,webinar,volunteer_training")
                .OldAnnotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .OldAnnotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .OldAnnotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .OldAnnotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .OldAnnotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "Donations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Donations",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "TargetEntity",
                table: "Donations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetEntityId",
                table: "Donations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Guardianships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<GuardianshipStatus>(type: "guardianship_status", nullable: false),
                    GraceUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guardianships", x => x.Id);
                    table.CheckConstraint("CK_Guardianships_StartDate", "\"StartDate\" <= NOW()");
                    table.ForeignKey(
                        name: "FK_Guardianships_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Guardianships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScopeType = table.Column<SubscriptionScope>(type: "subscription_scope", nullable: false),
                    ScopeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Provider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProviderSubscriptionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<SubscriptionStatus>(type: "subscription_status", nullable: false),
                    NextChargeAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastChargeAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CanceledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentSubscriptions_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuardianshipDonations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    GuardianshipId = table.Column<Guid>(type: "uuid", nullable: false),
                    DonationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DonationId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuardianshipDonations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuardianshipDonations_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuardianshipDonations_Donations_DonationId1",
                        column: x => x.DonationId1,
                        principalTable: "Donations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GuardianshipDonations_Guardianships_GuardianshipId",
                        column: x => x.GuardianshipId,
                        principalTable: "Guardianships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_TargetEntity_TargetEntityId",
                table: "Donations",
                columns: new[] { "TargetEntity", "TargetEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_GuardianshipDonations_DonationId",
                table: "GuardianshipDonations",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_GuardianshipDonations_DonationId1",
                table: "GuardianshipDonations",
                column: "DonationId1");

            migrationBuilder.CreateIndex(
                name: "IX_GuardianshipDonations_GuardianshipId_DonationId",
                table: "GuardianshipDonations",
                columns: new[] { "GuardianshipId", "DonationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guardianships_AnimalId",
                table: "Guardianships",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Guardianships_Status",
                table: "Guardianships",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Guardianships_UserId",
                table: "Guardianships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSubscriptions_PaymentMethodId",
                table: "PaymentSubscriptions",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSubscriptions_ProviderSubscriptionId",
                table: "PaymentSubscriptions",
                column: "ProviderSubscriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSubscriptions_Scope",
                table: "PaymentSubscriptions",
                columns: new[] { "UserId", "ScopeType", "ScopeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuardianshipDonations");

            migrationBuilder.DropTable(
                name: "PaymentSubscriptions");

            migrationBuilder.DropTable(
                name: "Guardianships");

            migrationBuilder.DropIndex(
                name: "IX_Donations_TargetEntity_TargetEntityId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "TargetEntity",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "TargetEntityId",
                table: "Donations");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:adoption_status", "pending,approved,rejected")
                .Annotation("Npgsql:Enum:aid_category", "food,medical,equipment,other")
                .Annotation("Npgsql:Enum:aid_status", "open,in_progress,fulfilled,cancelled")
                .Annotation("Npgsql:Enum:animal_gender", "male,female,unknown")
                .Annotation("Npgsql:Enum:animal_size", "small,medium,medium_plus,large")
                .Annotation("Npgsql:Enum:animal_status", "available,adopted,reserved,in_treatment,dead,euthanized")
                .Annotation("Npgsql:Enum:animal_temperament", "friendly,shy,needs_socialization,independent,affectionate,protective,curious,playful,calm,energetic,gentle,vocal,quiet,cuddly,nervous,confident,food_motivated,trainable,stubborn,good_with_kids,good_with_other_animals,needs_experienced_owner,senior_and_relaxed,young_and_learning,special_needs,bonded_pair")
                .Annotation("Npgsql:Enum:article_status", "draft,published,archived")
                .Annotation("Npgsql:Enum:audit_operation", "insert,update,delete")
                .Annotation("Npgsql:Enum:comment_status", "pending,approved,rejected")
                .Annotation("Npgsql:Enum:donation_status", "pending,completed,failed")
                .Annotation("Npgsql:Enum:event_status", "planned,ongoing,completed,cancelled")
                .Annotation("Npgsql:Enum:event_type", "adoption_day,fundraiser,webinar,volunteer_training")
                .Annotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .Annotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .Annotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .Annotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .Annotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:Enum:adoption_status", "pending,approved,rejected")
                .OldAnnotation("Npgsql:Enum:aid_category", "food,medical,equipment,other")
                .OldAnnotation("Npgsql:Enum:aid_status", "open,in_progress,fulfilled,cancelled")
                .OldAnnotation("Npgsql:Enum:animal_gender", "male,female,unknown")
                .OldAnnotation("Npgsql:Enum:animal_size", "small,medium,medium_plus,large")
                .OldAnnotation("Npgsql:Enum:animal_status", "available,adopted,reserved,in_treatment,dead,euthanized")
                .OldAnnotation("Npgsql:Enum:animal_temperament", "friendly,shy,needs_socialization,independent,affectionate,protective,curious,playful,calm,energetic,gentle,vocal,quiet,cuddly,nervous,confident,food_motivated,trainable,stubborn,good_with_kids,good_with_other_animals,needs_experienced_owner,senior_and_relaxed,young_and_learning,special_needs,bonded_pair")
                .OldAnnotation("Npgsql:Enum:article_status", "draft,published,archived")
                .OldAnnotation("Npgsql:Enum:audit_operation", "insert,update,delete")
                .OldAnnotation("Npgsql:Enum:comment_status", "pending,approved,rejected")
                .OldAnnotation("Npgsql:Enum:donation_status", "pending,completed,failed")
                .OldAnnotation("Npgsql:Enum:event_status", "planned,ongoing,completed,cancelled")
                .OldAnnotation("Npgsql:Enum:event_type", "adoption_day,fundraiser,webinar,volunteer_training")
                .OldAnnotation("Npgsql:Enum:guardianship_status", "requires_payment,active,completed")
                .OldAnnotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .OldAnnotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .OldAnnotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .OldAnnotation("Npgsql:Enum:subscription_scope", "global,aid_request,guardianship")
                .OldAnnotation("Npgsql:Enum:subscription_status", "active,canceled,paused")
                .OldAnnotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .OldAnnotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "Donations",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Donations",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2);
        }
    }
}
