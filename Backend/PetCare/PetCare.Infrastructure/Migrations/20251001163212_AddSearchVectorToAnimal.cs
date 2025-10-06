using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchVectorToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Animals",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "\r\n            to_tsvector('simple', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n            || to_tsvector('english', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n        ",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_SearchVector",
                table: "Animals",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Animals_SearchVector",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Animals");
        }
    }
}
