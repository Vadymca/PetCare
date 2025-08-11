namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="Email"/> value object.
/// </summary>
public class EmailTests
{
    /// <summary>
    /// Valid email should create instance.
    /// </summary>
    /// <param name="input">Valid email address string.</param>
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("john.doe@domain.org")]
    [InlineData("user-name@sub.domain.co")]
    public void Create_WithValidEmail_ShouldCreateInstance(string input)
    {
        // Act
        Email email = Email.Create(input);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be(input.Trim().ToLowerInvariant());
        email.ToString().Should().Be(email.Value);
    }

    /// <summary>
    /// Invalid email should throw exception.
    /// </summary>
    /// <param name="input">Invalid email address string.</param>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("not-an-email")]
    [InlineData("missing@domain")]
    [InlineData("noatsymbol.com")]
    [InlineData("@missinguser.com")]
    [InlineData(null)]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException(string? input)
    {
        // Act
        Action act = () => Email.Create(input!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Неправильний формат електронної пошти.*")
            .And.ParamName.Should().Be("email");
    }

    /// <summary>
    /// Normalize should trim and lowercase email.
    /// </summary>
    [Fact]
    public void Normalize_ShouldReturnLowercaseTrimmedEmail()
    {
        // Arrange
        const string rawEmail = "  USER@Example.COM  ";

        // Act
        string normalized = Email.Normalize(rawEmail);

        // Assert
        normalized.Should().Be("user@example.com");
    }

    /// <summary>
    /// IsValid should return true for valid emails.
    /// </summary>
    [Fact]
    public void IsValid_WithValidEmail_ShouldReturnTrue()
    {
        // Act
        bool result = Email.IsValid("valid.email@domain.com");

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// IsValid should return false for invalid emails.
    /// </summary>
    /// <param name="email">Invalid email address string.</param>
    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    [InlineData("bad@")]
    [InlineData("user@domain")]
    public void IsValid_WithInvalidEmail_ShouldReturnFalse(string email)
    {
        // Act
        bool result = Email.IsValid(email);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Equality for identical email values.
    /// </summary>
    [Fact]
    public void Equality_SameValue_ShouldBeEqual()
    {
        // Arrange
        Email e1 = Email.Create("Test@Email.com");
        Email e2 = Email.Create("  test@email.com  ");

        // Act & Assert
        e1.Should().Be(e2);
        e1.GetHashCode().Should().Be(e2.GetHashCode());
    }

    /// <summary>
    /// Equality for different email values.
    /// </summary>
    [Fact]
    public void Equality_DifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        Email e1 = Email.Create("first@domain.com");
        Email e2 = Email.Create("second@domain.com");

        // Act & Assert
        e1.Should().NotBe(e2);
    }
}
