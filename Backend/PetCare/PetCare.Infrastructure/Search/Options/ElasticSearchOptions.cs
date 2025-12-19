namespace PetCare.Infrastructure.Search.Options;

/// <summary>
/// Elasticsearch connection options.
/// </summary>
public sealed class ElasticSearchOptions
{
    /// <summary>
    /// Gets the Elasticsearch server URL.
    /// </summary>
    public string Url { get; init; } = null!;

    /// <summary>
    /// Gets the name of the Elasticsearch index to use.
    /// </summary>
    public string IndexName { get; init; } = null!;

    /// <summary>
    /// Gets the username for Elasticsearch authentication.
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    /// Gets the password for Elasticsearch authentication.
    /// </summary>
    public string? Password { get; init; }
}
