using Microsoft.EntityFrameworkCore.Migrations;
using PetCare.Domain.Enums;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCareCostToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CareCost",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: (int)AnimalCareCost.SixHundred);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareCost",
                table: "Animals");
        }
    }
}
