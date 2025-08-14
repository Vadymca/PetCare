namespace PetCare.Domain.Entities;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;

/// <summary>
/// Represents a gamification reward in the system.
/// </summary>
public sealed class GamificationReward : BaseEntity
{
    private GamificationReward()
    {
    }

    private GamificationReward(
        Guid userId,
        Guid? taskId,
        int points,
        string? description,
        bool used,
        DateTime awardedAt)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор користувача не може бути порожнім.", nameof(userId));
        }

        if (points < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(points), "Бали не можуть бути від'ємними.");
        }

        this.UserId = userId;
        this.TaskId = taskId;
        this.Points = points;
        this.Description = description;
        this.Used = used;
        this.AwardedAt = awardedAt;
    }

    /// <summary>
    /// Gets the number of points awarded.
    /// </summary>
    public int Points { get; private set; }

    /// <summary>
    /// Gets the description of the reward, if any. Can be null.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the reward has been used.
    /// </summary>
    public bool Used { get; private set; }

    /// <summary>
    /// Gets the date and time when the reward was awarded.
    /// </summary>
    public DateTime AwardedAt { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the user receiving the reward.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets navigation property to the user who received the reward.
    /// </summary>
    public User? User { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the task associated with the reward, if any. Can be null.
    /// </summary>
    public Guid? TaskId { get; private set; }

    /// <summary>
    /// Gets navigation property to the task associated with the reward, if any.
    /// </summary>
    public VolunteerTask? Task { get; private set; }

    /// <summary>
    /// Creates a new <see cref="GamificationReward"/> instance with the specified parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the user receiving the reward.</param>
    /// <param name="taskId">The unique identifier of the task associated with the reward, if any. Can be null.</param>
    /// <param name="points">The number of points awarded.</param>
    /// <param name="description">The description of the reward, if any. Can be null.</param>
    /// <param name="used">Indicates whether the reward has been used. Defaults to false.</param>
    /// <returns>A new instance of <see cref="GamificationReward"/> with the specified parameters.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is an empty GUID.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="points"/> is negative.</exception>
    public static GamificationReward Create(
        Guid userId,
        Guid? taskId,
        int points,
        string? description = null,
        bool used = false)
    {
        return new GamificationReward(
            userId,
            taskId,
            points,
            description,
            used,
            DateTime.UtcNow);
    }

    /// <summary>
    /// Marks the reward as used.
    /// </summary>
    public void MarkAsUsed()
    {
        this.Used = true;
    }

    /// <summary>
    /// Updates the description of the reward.
    /// </summary>
    /// <param name="description">The new description of the reward. Can be null.</param>
    public void UpdateDescription(string? description)
    {
        this.Description = description;
    }
}
