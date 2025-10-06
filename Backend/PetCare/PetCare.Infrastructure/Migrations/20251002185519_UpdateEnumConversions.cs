using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnumConversions : Migration
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
                .OldAnnotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .OldAnnotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .OldAnnotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .OldAnnotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .OldAnnotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:adoption_status", "pending,approved,rejected")
                .Annotation("Npgsql:Enum:aid_category", "food,medical,equipment,other")
                .Annotation("Npgsql:Enum:aid_status", "open,in_progress,fulfilled,cancelled")
                .Annotation("Npgsql:Enum:animal_gender.animal_gender", "male,female,unknown")
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
                .OldAnnotation("Npgsql:Enum:io_t_device_status", "active,inactive,error")
                .OldAnnotation("Npgsql:Enum:io_t_device_type", "feeder,temperature,camera")
                .OldAnnotation("Npgsql:Enum:lost_pet_status", "lost,found,reunited")
                .OldAnnotation("Npgsql:Enum:user_role", "user,admin,moderator,shelter_manager,veterinarian,volunteer")
                .OldAnnotation("Npgsql:Enum:volunteer_task_status", "open,in_progress,completed,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }
    }
}
