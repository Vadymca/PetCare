namespace PetCare.Infrastructure.Services.Zipcodebase;

using System.Collections.Generic;

/// <summary>
/// Represents the response from Zipcodebase API.
/// </summary>
public sealed record ZipcodebaseResponse(Dictionary<string, List<ZipcodebaseResult>> Results);
