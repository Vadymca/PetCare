using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PetCare.Domain.Enums;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentIntent : Migration
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
                .Annotation("Npgsql:Enum:payment_intent_status", "pending,succeeded,failed,canceled")
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
                .OldAnnotation("Npgsql:Enum:guardianship_status", "requires_payment,active,completed")
                .OldAnnotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .OldAnnotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .OldAnnotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .OldAnnotation("Npgsql:Enum:subscription_scope", "global,aid_request,guardianship")
                .OldAnnotation("Npgsql:Enum:subscription_status", "active,canceled,paused")
                .OldAnnotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .OldAnnotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "PaymentIntents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalOrderId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PaymentProvider = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ProviderPaymentId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ScopeType = table.Column<SubscriptionScope>(type: "subscription_scope", nullable: true),
                    ScopeId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    Anonymous = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<PaymentIntentStatus>(type: "payment_intent_status", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DonationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    GuardianshipId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentIntents", x => x.Id);
                    table.CheckConstraint("CK_PaymentIntents_Amount", "\"Amount\" > 0");
                    table.ForeignKey(
                        name: "FK_PaymentIntents_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentIntents_Guardianships_GuardianshipId",
                        column: x => x.GuardianshipId,
                        principalTable: "Guardianships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentIntents_PaymentSubscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "PaymentSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentIntents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_DonationId",
                table: "PaymentIntents",
                column: "DonationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_ExternalOrderId",
                table: "PaymentIntents",
                column: "ExternalOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_GuardianshipId",
                table: "PaymentIntents",
                column: "GuardianshipId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_ScopeId",
                table: "PaymentIntents",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_ScopeType",
                table: "PaymentIntents",
                column: "ScopeType");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_Status",
                table: "PaymentIntents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_SubscriptionId",
                table: "PaymentIntents",
                column: "SubscriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentIntents_UserId",
                table: "PaymentIntents",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentIntents");

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
                .OldAnnotation("Npgsql:Enum:guardianship_status", "requires_payment,active,completed")
                .OldAnnotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .OldAnnotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .OldAnnotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .OldAnnotation("Npgsql:Enum:payment_intent_status", "pending,succeeded,failed,canceled")
                .OldAnnotation("Npgsql:Enum:subscription_scope", "global,aid_request,guardianship")
                .OldAnnotation("Npgsql:Enum:subscription_status", "active,canceled,paused")
                .OldAnnotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .OldAnnotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }
    }
}
