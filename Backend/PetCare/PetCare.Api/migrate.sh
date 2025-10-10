#!/bin/sh
echo "Waiting for database..."
until nc -z db 5432; do
  sleep 2
done
echo "Database ready. Applying migrations..."
dotnet ef database update \
  --project /src/PetCare.Infrastructure/PetCare.Infrastructure.csproj \
  --startup-project /src/PetCare.Api/PetCare.Api.csproj
echo "Migrations applied."
