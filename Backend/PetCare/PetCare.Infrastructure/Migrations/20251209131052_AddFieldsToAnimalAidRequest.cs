using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToAnimalAidRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "AnimalAidRequests",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUrgent",
                table: "AnimalAidRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "AnimalAidRequests",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "AnimalAidRequests",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidRequests_Slug",
                table: "AnimalAidRequests",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnimalAidRequests_Slug",
                table: "AnimalAidRequests");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "AnimalAidRequests");

            migrationBuilder.DropColumn(
                name: "IsUrgent",
                table: "AnimalAidRequests");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "AnimalAidRequests");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "AnimalAidRequests");
        }
    }
}
