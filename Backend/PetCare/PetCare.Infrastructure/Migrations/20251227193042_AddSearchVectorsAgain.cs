using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchVectorsAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Species",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector('simple', coalesce(\"Name\", ''))\r\n||\r\nto_tsvector('english', coalesce(\"Name\", ''))",
                stored: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Breeds",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector('simple', coalesce(\"Name\", ''))\r\n||\r\nto_tsvector('english', coalesce(\"Name\", ''))",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_SearchVector",
                table: "Species",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_SearchVector",
                table: "Breeds",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Species_SearchVector",
                table: "Species");

            migrationBuilder.DropIndex(
                name: "IX_Breeds_SearchVector",
                table: "Breeds");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Breeds");
        }
    }
}
