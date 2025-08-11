namespace PetCare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Infrastructure.Persistence.Configurations;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
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

    /// <summary>
    /// Configures the model by applying entity configurations.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("animal_gender", new[] { "Male", "Female", "Unknown" });
        modelBuilder.HasPostgresEnum("animal_status", new[] { "Available", "Adopted", "Reserved", "InTreatment", " Dead", "Euthanized" });
        modelBuilder.HasPostgresEnum("user_role", new[] { "User", "Admin", "Moderator" });

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new SpeciesConfiguration());
        modelBuilder.ApplyConfiguration(new BreedConfiguration());
        modelBuilder.ApplyConfiguration(new ShelterConfiguration());
        modelBuilder.ApplyConfiguration(new AnimalConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
