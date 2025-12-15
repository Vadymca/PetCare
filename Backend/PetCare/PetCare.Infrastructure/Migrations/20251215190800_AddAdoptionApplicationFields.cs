using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdoptionApplicationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdoptionDate",
                table: "AdoptionApplications",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<string>(
                name: "CuratorName",
                table: "AdoptionApplications",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CuratorPhone",
                table: "AdoptionApplications",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MeetingDate",
                table: "AdoptionApplications",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionDate",
                table: "AdoptionApplications",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_AdoptionDate",
                table: "AdoptionApplications",
                column: "AdoptionDate");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_MeetingDate",
                table: "AdoptionApplications",
                column: "MeetingDate");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionApplications_RejectionDate",
                table: "AdoptionApplications",
                column: "RejectionDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdoptionApplications_AdoptionDate",
                table: "AdoptionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdoptionApplications_MeetingDate",
                table: "AdoptionApplications");

            migrationBuilder.DropIndex(
                name: "IX_AdoptionApplications_RejectionDate",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "AdoptionDate",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "CuratorName",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "CuratorPhone",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "MeetingDate",
                table: "AdoptionApplications");

            migrationBuilder.DropColumn(
                name: "RejectionDate",
                table: "AdoptionApplications");
        }
    }
}
