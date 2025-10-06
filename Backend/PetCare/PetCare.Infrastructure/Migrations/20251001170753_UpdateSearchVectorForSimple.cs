using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSearchVectorForSimple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                oldComputedColumnSql: "\r\n            to_tsvector('ukrainian', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n            || to_tsvector('english', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n        ",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Animals",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "\r\n            to_tsvector('ukrainian', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n            || to_tsvector('english', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n        ",
                stored: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true,
                oldComputedColumnSql: "\r\n            to_tsvector('simple', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n            || to_tsvector('english', coalesce(\"Name\",'') || ' ' || coalesce(\"Description\",''))\r\n        ",
                oldStored: true);
        }
    }
}
