namespace PetCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Abstractions.Events;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Common;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Events;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class AppDbContext : DbContext
{
    private readonly IDomainEventDispatcher dispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    /// <param name="dispatcher">The dispatcher to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher dispatcher)
        : base(options)
    {
        this.dispatcher = dispatcher;
    }

    /// <summary>
    /// Gets the adoptionAplication entities.
    /// </summary>
    public DbSet<AdoptionApplication> AdoptionApplications => this.Set<AdoptionApplication>();

    /// <summary>
    /// Gets the animal entities.
    /// </summary>
    public DbSet<Animal> Animals => this.Set<Animal>();

    /// <summary>
    /// Gets the shelter entities.
    /// </summary>
    public DbSet<Shelter> Shelters => this.Set<Shelter>();

    /// <summary>
    /// Gets the specie entities.
    /// </summary>
    public DbSet<Specie> Species => this.Set<Specie>();

    /// <summary>
    /// Gets the user entities.
    /// </summary>
    public DbSet<User> Users => this.Set<User>();

    /// <summary>
    /// Gets the volunteerTask entities.
    /// </summary>
    public DbSet<VolunteerTask> VolunteerTasks => this.Set<VolunteerTask>();

    /// <summary>
    /// Gets the animalAidDonation entities.
    /// </summary>
    public DbSet<AnimalAidDonation> AnimalAidDonations => this.Set<AnimalAidDonation>();

    /// <summary>
    /// Gets the animalAidRequest entities.
    /// </summary>
    public DbSet<AnimalAidRequest> AnimalAidRequests => this.Set<AnimalAidRequest>();

    /// <summary>
    /// Gets the animalSubscription entities.
    /// </summary>
    public DbSet<AnimalSubscription> AnimalSubscriptions => this.Set<AnimalSubscription>();

    /// <summary>
    /// Gets the article entities.
    /// </summary>
    public DbSet<Article> Articles => this.Set<Article>();

    /// <summary>
    /// Gets the articleComment entities.
    /// </summary>
    public DbSet<ArticleComment> ArticleComments => this.Set<ArticleComment>();

    /// <summary>
    /// Gets the auditLog entities.
    /// </summary>
    public DbSet<AuditLog> AuditLogs => this.Set<AuditLog>();

    /// <summary>
    /// Gets the breed entities.
    /// </summary>
    public DbSet<Breed> Breeds => this.Set<Breed>();

    /// <summary>
    /// Gets the category entities.
    /// </summary>
    public DbSet<Category> Categories => this.Set<Category>();

    /// <summary>
    /// Gets the donation entities.
    /// </summary>
    public DbSet<Donation> Donations => this.Set<Donation>();

    /// <summary>
    /// Gets the event entities.
    /// </summary>
    public DbSet<Event> Events => this.Set<Event>();

    /// <summary>
    /// Gets the eventParticipant entities.
    /// </summary>
    public DbSet<EventParticipant> EventParticipants => this.Set<EventParticipant>();

    /// <summary>
    /// Gets the gamificationReward entities.
    /// </summary>
    public DbSet<GamificationReward> GamificationRewards => this.Set<GamificationReward>();

    /// <summary>
    /// Gets the ioTDevice entities.
    /// </summary>
    public DbSet<IoTDevice> IoTDevices => this.Set<IoTDevice>();

    /// <summary>
    /// Gets the like entities.
    /// </summary>
    public DbSet<Like> Likes => this.Set<Like>();

    /// <summary>
    /// Gets the lostPet entities.
    /// </summary>
    public DbSet<LostPet> LostPets => this.Set<LostPet>();

    /// <summary>
    /// Gets the notification entities.
    /// </summary>
    public DbSet<Notification> Notifications => this.Set<Notification>();

    /// <summary>
    /// Gets the notificationType entities.
    /// </summary>
    public DbSet<NotificationType> NotificationTypes => this.Set<NotificationType>();

    /// <summary>
    /// Gets the paymentMethod entities.
    /// </summary>
    public DbSet<PaymentMethod> PaymentMethods => this.Set<PaymentMethod>();

    /// <summary>
    /// Gets the shelterSubscription entities.
    /// </summary>
    public DbSet<ShelterSubscription> ShelterSubscriptions => this.Set<ShelterSubscription>();

    /// <summary>
    /// Gets the successStory entities.
    /// </summary>
    public DbSet<SuccessStory> SuccessStories => this.Set<SuccessStory>();

    /// <summary>
    /// Gets the tag entities.
    /// </summary>
    public DbSet<Tag> Tags => this.Set<Tag>();

    /// <summary>
    /// Gets the volunteerTaskAssignment entities.
    /// </summary>
    public DbSet<VolunteerTaskAssignment> VolunteerTaskAssignments => this.Set<VolunteerTaskAssignment>();

    /// <inheritdoc/>
    /// <summary>
    /// Saves all changes made in this context to the database.
    /// In addition to persisting data, this method collects all domain events
    /// from tracked aggregate roots, dispatches them using the configured
    /// <see cref="IDomainEventDispatcher"/>, and then clears the events
    /// from the entities to prevent re-dispatching.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = this.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await this.dispatcher.DispatchAsync(domainEvents, cancellationToken).ConfigureAwait(false);

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        return result;
    }

    /// <summary>
    /// Configures the model by applying entity configurations.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.HasPostgresEnum<AdoptionStatus>();
        modelBuilder.HasPostgresEnum<AidCategory>();
        modelBuilder.HasPostgresEnum<AidStatus>();
        modelBuilder.HasPostgresEnum<AnimalGender>();
        modelBuilder.HasPostgresEnum<AnimalStatus>();
        modelBuilder.HasPostgresEnum<ArticleStatus>();
        modelBuilder.HasPostgresEnum<AuditOperation>();
        modelBuilder.HasPostgresEnum<CommentStatus>();
        modelBuilder.HasPostgresEnum<DonationStatus>();
        modelBuilder.HasPostgresEnum<EventStatus>();
        modelBuilder.HasPostgresEnum<EventType>();
        modelBuilder.HasPostgresEnum<IoTDeviceStatus>();
        modelBuilder.HasPostgresEnum<IoTDeviceType>();
        modelBuilder.HasPostgresEnum<LostPetStatus>();
        modelBuilder.HasPostgresEnum<UserRole>();
        modelBuilder.HasPostgresEnum<VolunteerTaskStatus>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
