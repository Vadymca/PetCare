namespace PetCare.Domain.Entities;
using PetCare.Domain.Common;

/// <summary>
/// Represents a type of notification used in the system.
/// </summary>
public sealed class NotificationType : BaseEntity
{
    private NotificationType()
    {
        this.Name = string.Empty;
    }

    private NotificationType(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Назва типу сповіщення не може бути порожньою.", nameof(name));
        }

        this.Name = name.Trim();
        this.Description = description?.Trim();
    }

    /// <summary>
    /// Gets the name of the notification type.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the description of the notification type, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Creates a new <see cref="NotificationType"/> instance with the specified parameters.
    /// </summary>
    /// <param name="name">The name of the notification type.</param>
    /// <param name="description">The description of the notification type, if any. Can be null.</param>
    /// <returns>A new instance of <see cref="NotificationType"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
    public static NotificationType Create(string name, string? description = null)
    {
        return new NotificationType(name, description);
    }

    /// <summary>
    /// Updates the notification type's name or description, if provided.
    /// </summary>
    /// <param name="name">The new name of the notification type, if provided. If null or whitespace, the name remains unchanged.</param>
    /// <param name="description">The new description of the notification type, if provided. If null, the description remains unchanged.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
    public void Update(string? name = null, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            this.Name = name.Trim();
        }

        if (description is not null)
        {
            this.Description = description.Trim();
        }
    }
}
