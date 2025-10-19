namespace PetCare.Api.Endpoints.Shelters
{
    /// <summary>
    /// Represents the body for adding a photo to a shelter.
    /// </summary>
    /// <param name="PhotoUrl">The URL of the photo to add.</param>
    public sealed record AddShelterPhotoBody(string PhotoUrl);
}
