using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditAndShelterTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Функція для оновлення кількості тварин у притулку
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION update_shelter_occupancy()
                RETURNS TRIGGER AS $$
                BEGIN
                    IF TG_OP = 'UPDATE' AND OLD.""ShelterId"" IS DISTINCT FROM NEW.""ShelterId"" THEN
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" - 1 WHERE ""Id"" = OLD.""ShelterId"";
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" + 1 WHERE ""Id"" = NEW.""ShelterId"";
                        RETURN NEW;
                    ELSIF TG_OP = 'INSERT' THEN
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" + 1 WHERE ""Id"" = NEW.""ShelterId"";
                        RETURN NEW;
                    ELSIF TG_OP = 'DELETE' THEN
                        UPDATE ""Shelters"" SET ""CurrentOccupancy"" = ""CurrentOccupancy"" - 1 WHERE ""Id"" = OLD.""ShelterId"";
                        RETURN OLD;
                    END IF;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                DROP TRIGGER IF EXISTS animal_shelter_trigger ON ""Animals"";

                CREATE TRIGGER animal_shelter_trigger
                AFTER INSERT OR DELETE OR UPDATE OF ""ShelterId"" ON ""Animals""
                FOR EACH ROW EXECUTE FUNCTION update_shelter_occupancy();
                ");

            // Функція для audit trigger
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION audit_trigger_function()
                RETURNS TRIGGER AS $$
                BEGIN
                    IF TG_OP = 'INSERT' THEN
                        INSERT INTO ""AuditLogs""(""TableName"", ""RecordId"", ""Operation"", ""UserId"", ""Changes"", ""CreatedAt"")
                        VALUES (TG_TABLE_NAME, NEW.""Id"", 'Insert', NULL, row_to_json(NEW)::jsonb, CURRENT_TIMESTAMP);
                        RETURN NEW;
                    ELSIF TG_OP = 'UPDATE' THEN
                        INSERT INTO ""AuditLogs""(""TableName"", ""RecordId"", ""Operation"", ""UserId"", ""Changes"", ""CreatedAt"")
                        VALUES (TG_TABLE_NAME, NEW.""Id"", 'Update', NULL, jsonb_build_object('old', row_to_json(OLD), 'new', row_to_json(NEW)), CURRENT_TIMESTAMP);
                        RETURN NEW;
                    ELSIF TG_OP = 'DELETE' THEN
                        INSERT INTO ""AuditLogs""(""TableName"", ""RecordId"", ""Operation"", ""UserId"", ""Changes"", ""CreatedAt"")
                        VALUES (TG_TABLE_NAME, OLD.""Id"", 'Delete', NULL, row_to_json(OLD)::jsonb, CURRENT_TIMESTAMP);
                        RETURN OLD;
                    END IF;
                END;
                $$ LANGUAGE plpgsql;
                ");

            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    tbl RECORD;
                    trigger_name TEXT;
                BEGIN
                    FOR tbl IN
                        SELECT tablename
                        FROM pg_tables
                        WHERE schemaname = 'public'
                          AND tablename NOT IN ('AuditLogs', '__EFMigrationsHistory')
                    LOOP
                        trigger_name := 'audit_trigger_' || tbl.tablename;

                        -- Створюємо тригер, якщо він ще не існує
                        IF NOT EXISTS (
                            SELECT 1
                            FROM pg_trigger
                            WHERE tgname = trigger_name
                        ) THEN
                            EXECUTE format(
                                'CREATE TRIGGER %I
                                 AFTER INSERT OR UPDATE OR DELETE ON %I
                                 FOR EACH ROW EXECUTE PROCEDURE audit_trigger_function();',
                                trigger_name,
                                tbl.tablename
                            );
                        END IF;
                    END LOOP;
                END
                $$;
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
