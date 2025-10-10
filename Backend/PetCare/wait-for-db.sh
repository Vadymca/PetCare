#!/bin/sh
set -e

host="$1"
port="$2"
shift 2

echo "Waiting for database $host:$port..."
while ! nc -z $host $port; do
  sleep 2
done
echo "Database ready!"

# Apply migrations
echo "Applying EF Core migrations..."
dotnet ef database update --project /src/PetCare.Infrastructure/PetCare.Infrastructure.csproj --startup-project /src/PetCare.Api/PetCare.Api.csproj
echo "Migrations applied."

# Run the application
exec dotnet PetCare.Api.dll "$@"
