namespace PetCare.Domain.FakeEntities;

using System.Collections.Generic;

/// <summary>
/// Represents a static page of the website.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="StaticPage"/> class.
/// </remarks>
public sealed class StaticPage(string route, Dictionary<string, string> title, Dictionary<string, string> text)
{
    /// <summary>
    /// Gets unique route of the static page.
    /// </summary>
    public string Route { get; init; } = route;

    /// <summary>
    /// Gets titles in different languages.
    /// </summary>
    public IReadOnlyDictionary<string, string> Title { get; init; } = new Dictionary<string, string>(title);

    /// <summary>
    /// Gets text content in different languages.
    /// </summary>
    public IReadOnlyDictionary<string, string> Text { get; init; } = new Dictionary<string, string>(text);
}
