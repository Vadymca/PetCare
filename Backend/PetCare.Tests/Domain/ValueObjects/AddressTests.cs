// <copyright file="AddressTests.cs" company="PetCare">
// Copyright (c) PetCare. All rights reserved.
// </copyright>

namespace PetCare.Tests.Domain.ValueObjects;
using FluentAssertions;
using PetCare.Domain.ValueObjects;
using System;
using Xunit;

/// <summary>
/// Unit tests for <see cref="Address"/> value object.
/// </summary>
public class AddressTests
{
    /// <summary>
    /// Tests that creating <see cref="Address"/> with a valid address string
    /// returns a new instance with the expected value.
    /// </summary>
    [Fact]
    public void Create_WithValidAddress_ShouldCreateInstance()
    {
        // Arrange
        const string validAddress = "вул. Головна, 12, м. Чернівці";

        // Act
        Address address = Address.Create(validAddress);

        // Assert
        address.Should().NotBeNull();
        address.Value.Should().Be(validAddress);
        address.ToString().Should().Be(validAddress);
    }

    /// <summary>
    /// Tests that creating <see cref="Address"/> with null or whitespace string
    /// throws an <see cref="ArgumentException"/> with the correct message and param name.
    /// </summary>
    /// <param name="invalidAddress">Invalid address string.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespace_ShouldThrowArgumentException(string invalidAddress)
    {
        // Act
        Action act = () => Address.Create(invalidAddress!);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage("Адреса не може бути порожньою*")
           .And.ParamName.Should().Be("address");
    }

    /// <summary>
    /// Tests that creating an Address with length less than 3
    /// throws an <see cref="ArgumentException"/> with the correct message and param name.
    /// </summary>
    /// <param name="invalidAddress">An invalid address string with length less than 3 but not whitespace.</param>
    [Theory]
    [InlineData("a")]
    [InlineData("12")]
    public void Create_WithTooShortAddress_ShouldThrowArgumentException(string invalidAddress)
    {
        // Act
        Action act = () => Address.Create(invalidAddress);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage("Адреса повинна містити від 3 до 200 символів*")
           .And.ParamName.Should().Be("address");
    }

    /// <summary>
    /// Tests that creating an Address with length exceeding 200 characters
    /// throws an <see cref="ArgumentException"/> with the correct message and param name.
    /// </summary>
    [Fact]
    public void Create_WithTooLongAddress_ShouldThrowArgumentException()
    {
        // Arrange
        string tooLong = new string('a', 201);

        // Act
        Action act = () => Address.Create(tooLong);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage("Адреса повинна містити від 3 до 200 символів*")
           .And.ParamName.Should().Be("address");
    }

    /// <summary>
    /// Tests that two <see cref="Address"/> instances with the same value
    /// are equal.
    /// </summary>
    [Fact]
    public void Equality_SameValue_ShouldBeEqual()
    {
        // Arrange
        const string value = "вул. Головна, 12, м. Чернівці";
        Address addr1 = Address.Create(value);
        Address addr2 = Address.Create(value);

        // Act & Assert
        addr1.Should().Be(addr2);
        addr1.GetHashCode().Should().Be(addr2.GetHashCode());
    }

    /// <summary>
    /// Tests that two <see cref="Address"/> instances with different values
    /// are not equal.
    /// </summary>
    [Fact]
    public void Equality_DifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        Address addr1 = Address.Create("вул. Головна, 12, м. Чернівці");
        Address addr2 = Address.Create("просп. Незалежності, 5, м. Київ");

        // Act & Assert
        addr1.Should().NotBe(addr2);
    }
}
