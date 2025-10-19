namespace PetCare.Api.Endpoints.Species
{
    /// <summary>
    /// Represents the data required to update the name of a species.
    /// </summary>
    /// <param name="NewName">The new name to assign to the species. Cannot be null or empty.</param>
    public sealed record UpdateSpecieBody(string NewName);
}
