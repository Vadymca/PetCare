namespace PetCare.Tests.Domain.Specifications;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.User;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Contains unit tests for <see cref="User"/> specifications.
/// Tests filtering by email, role, and shelter subscriptions.
/// </summary>
public class UserSpecificationTests
{
    /// <summary>
    /// Tests that <see cref="UserByEmailSpecification"/> correctly filters users by email.
    /// </summary>
    [Fact]
    public void UserByEmailSpecification_ShouldFilterByEmail()
    {
        // Arrange
        var user1 = CreateUser("a@b.com", UserRole.User);
        var user2 = CreateUser("c@d.com", UserRole.User);
        var users = new List<User> { user1, user2 };
        var spec = new UserByEmailSpecification("c@d.com");

        // Act
        var result = users.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("c@d.com", result[0].Email.Value);
    }

    /// <summary>
    /// Tests that <see cref="UsersByRoleSpecification"/> correctly filters users by role.
    /// </summary>
    [Fact]
    public void UsersByRoleSpecification_ShouldFilterByRole()
    {
        // Arrange
        var user1 = CreateUser("a@b.com", UserRole.User);
        var user2 = CreateUser("c@d.com", UserRole.Admin);
        var users = new List<User> { user1, user2 };
        var spec = new UsersByRoleSpecification(UserRole.Admin);

        // Act
        var result = users.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(UserRole.Admin, result[0].Role);
    }

    /// <summary>
    /// Tests that <see cref="UsersByShelterSubscriptionSpecification"/> correctly filters users by shelter subscription.
    /// </summary>
    [Fact]
    public void UsersByShelterSubscriptionSpecification_ShouldFilterByShelter()
    {
        // Arrange
        var shelterId1 = Guid.NewGuid();
        var shelterId2 = Guid.NewGuid();
        var user1 = CreateUser("a@b.com", UserRole.User, shelterId1);
        var user2 = CreateUser("c@d.com", UserRole.User, shelterId2);
        var users = new List<User> { user1, user2 };
        var spec = new UsersByShelterSubscriptionSpecification(shelterId1);

        // Act
        var result = users.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Contains(result, u => u.ShelterSubscriptions.Any(s => s.ShelterId == shelterId1));
    }

    /// <summary>
    /// Tests that <see cref="UserByEmailSpecification"/> throws an exception when an invalid email is provided.
    /// </summary>
    [Fact]
    public void UserByEmailSpecification_ShouldThrow_WhenEmailInvalid()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new UserByEmailSpecification(string.Empty));
        Assert.Contains("Email не може бути нульовим або порожнім", ex.Message);
    }

    /// <summary>
    /// Tests that <see cref="UsersByShelterSubscriptionSpecification"/> throws an exception when an empty shelter ID is provided.
    /// </summary>
    [Fact]
    public void UsersByShelterSubscriptionSpecification_ShouldThrow_WhenShelterIdEmpty()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new UsersByShelterSubscriptionSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор притулку не може бути порожнім", ex.Message);
    }

    /// <summary>
    /// Creates a test user with optional shelter subscription.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="role">The role of the user.</param>
    /// <param name="shelterId">Optional shelter ID to subscribe the user to.</param>
    /// <returns>A new <see cref="User"/> instance.</returns>
    private static User CreateUser(string email, UserRole role, Guid shelterId = default)
    {
        var user = User.Create(email, "pass", "F", "L", "+380501112233", role);

        if (shelterId != Guid.Empty)
        {
            // Use the factory method to create a shelter subscription
            var subscription = ShelterSubscription.Create(user.Id, shelterId);
            user.AddShelterSubscription(subscription, user.Id);
        }

        return user;
    }
}
