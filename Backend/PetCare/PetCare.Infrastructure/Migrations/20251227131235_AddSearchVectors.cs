using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchVectors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Shelters",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector('simple',\r\n    coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", '')\r\n)\r\n||\r\nto_tsvector('english',\r\n    coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", '')\r\n)",
                stored: true);

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Animals",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector('simple',\r\n    coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", '')\r\n)\r\n||\r\nto_tsvector('english',\r\n    coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", '')\r\n)",
                stored: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true,
                oldComputedColumnSql: "\r\n            to_tsvector('simple', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n            || to_tsvector('english', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n        ",
                oldStored: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "AnimalAidRequests",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector('simple',\r\n    coalesce(\"Title\", '') || ' ' || coalesce(\"Description\", '')\r\n)\r\n||\r\nto_tsvector('english',\r\n    coalesce(\"Title\", '') || ' ' || coalesce(\"Description\", '')\r\n)",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shelters_SearchVector",
                table: "Shelters",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAidRequests_SearchVector",
                table: "AnimalAidRequests",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Shelters_SearchVector",
                table: "Shelters");

            migrationBuilder.DropIndex(
                name: "IX_AnimalAidRequests_SearchVector",
                table: "AnimalAidRequests");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Shelters");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "AnimalAidRequests");

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Animals",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "\r\n            to_tsvector('simple', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n            || to_tsvector('english', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n        ",
                stored: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true,
                oldComputedColumnSql: "to_tsvector('simple',\r\n    coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", '')\r\n)\r\n||\r\nto_tsvector('english',\r\n    coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", '')\r\n)",
                oldStored: true);
        }
    }
}
