using PetCare.Domain.Common;

namespace PetCare.Domain.ValueObjects
{
    public sealed class Coordinates : ValueObject
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        // Parameterless constructor for EF Core
        private Coordinates() 
        { 
            Latitude = 0;
            Longitude = 0;
        }

        private Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static Coordinates Create(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("Широта повинна бути між -90 та 90 градусами.", nameof(latitude));

            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Довгота повинна бути між -180 та 180 градусами.", nameof(longitude));

            return new Coordinates(latitude, longitude);
        }

        /// <summary>
        /// Calculate distance to another point in kilometers using Haversine formula
        /// </summary>
        public double DistanceTo(Coordinates other)
        {
            const double earthRadiusKm = 6371.0;

            var lat1Rad = ToRadians(Latitude);
            var lat2Rad = ToRadians(other.Latitude);
            var deltaLatRad = ToRadians(other.Latitude - Latitude);
            var deltaLonRad = ToRadians(other.Longitude - Longitude);

            var a = Math.Sin(deltaLatRad / 2) * Math.Sin(deltaLatRad / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(deltaLonRad / 2) * Math.Sin(deltaLonRad / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadiusKm * c;
        }

        private static double ToRadians(double degrees) => degrees * Math.PI / 180;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Math.Round(Latitude, 6); // Round to ~1 meter precision
            yield return Math.Round(Longitude, 6);
        }

        public override string ToString() => $"{Latitude:F6}, {Longitude:F6}";

        /// <summary>
        /// Returns coordinates in Well-Known Text (WKT) format for PostGIS
        /// </summary>
        public string ToWkt() => $"POINT({Longitude:F6} {Latitude:F6})";
    }
}
