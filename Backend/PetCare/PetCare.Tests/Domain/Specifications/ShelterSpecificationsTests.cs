namespace PetCare.Tests.Domain.Specifications;

using System;
using System.Collections.Generic;
using System.Linq;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;
using PetCare.Domain.Enums;
using PetCare.Domain.Specifications.Shelter;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Unit tests for shelter specifications.
/// </summary>
public class ShelterSpecificationsTests
{
    /// <summary>
    /// Tests that ShelterByDeviceSpecification returns only shelters containing the specified device.
    /// </summary>
    [Fact]
    public void ShelterByDeviceSpecification_ShouldReturnSheltersWithDevice()
    {
        // Arrange
        var managerAndUserId = Guid.NewGuid();
        var shelterWithDevice = this.CreateShelter(managerId: managerAndUserId, requestingUserId: managerAndUserId);

        var device = IoTDevice.Create(
            shelterId: shelterWithDevice.Id,
            type: IoTDeviceType.Temperature,
            name: "Device1",
            status: IoTDeviceStatus.Active,
            serialNumber: "SN123456");

        shelterWithDevice.AddIoTDevice(device, managerAndUserId);

        var shelterWithoutDevice = this.CreateShelter();

        var spec = new ShelterByDeviceSpecification(device.Id);
        var shelters = new List<Shelter> { shelterWithDevice, shelterWithoutDevice };

        // Act
        var result = shelters.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Contains(device, result[0].IoTDevices);
    }

    /// <summary>
    /// Tests that ShelterByDeviceSpecification throws an exception when initialized with an empty GUID.
    /// </summary>
    [Fact]
    public void ShelterByDeviceSpecification_ShouldThrow_OnEmptyGuid()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new ShelterByDeviceSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор пристрою не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Tests that SheltersByManagerSpecification returns only shelters managed by the specified manager.
    /// </summary>
    [Fact]
    public void SheltersByManagerSpecification_ShouldReturnSheltersWithManager()
    {
        // Arrange
        var managerId = Guid.NewGuid();
        var shelterWithManager = this.CreateShelter(managerId: managerId);
        var shelterWithoutManager = this.CreateShelter();

        var spec = new SheltersByManagerSpecification(managerId);
        var shelters = new List<Shelter> { shelterWithManager, shelterWithoutManager };

        // Act
        var result = shelters.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(managerId, result[0].ManagerId);
    }

    /// <summary>
    /// Tests that SheltersByManagerSpecification throws an exception when initialized with an empty GUID.
    /// </summary>
    [Fact]
    public void SheltersByManagerSpecification_ShouldThrow_OnEmptyGuid()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new SheltersByManagerSpecification(Guid.Empty));
        Assert.Contains("Ідентифікатор менеджера не може бути порожнім.", ex.Message);
    }

    /// <summary>
    /// Tests that SheltersWithFreeCapacitySpecification returns only shelters with available capacity.
    /// </summary>
    [Fact]
    public void SheltersWithFreeCapacitySpecification_ShouldReturnOnlySheltersWithFreeCapacity()
    {
        // Arrange
        var shelterWithSpace = this.CreateShelter(capacity: 10, currentOccupancy: 5);
        var shelterFull = this.CreateShelter(capacity: 5, currentOccupancy: 5);

        var spec = new SheltersWithFreeCapacitySpecification();
        var shelters = new List<Shelter> { shelterWithSpace, shelterFull };

        // Act
        var result = shelters.AsQueryable().Where(spec.ToExpression()).ToList();

        // Assert
        Assert.Single(result);
        Assert.True(result[0].CurrentOccupancy < result[0].Capacity);
    }

    private Shelter CreateShelter(
     Guid? managerId = null,
     List<IoTDevice>? devices = null,
     int capacity = 10,
     int currentOccupancy = 0,
     Guid? requestingUserId = null)
    {
        var shelter = Shelter.Create(
            name: "Test Shelter",
            address: "Test Address",
            coordinates: Coordinates.Origin,
            contactPhone: "+380501112233",
            contactEmail: "test@shelter.com",
            description: "Test Description",
            capacity: capacity,
            currentOccupancy: currentOccupancy,
            photos: new List<string>(),
            virtualTourUrl: null,
            workingHours: null,
            socialMedia: new Dictionary<string, string>(),
            managerId: managerId ?? Guid.NewGuid());

        if (devices != null && requestingUserId.HasValue)
        {
            foreach (var device in devices)
            {
                shelter.AddIoTDevice(device, requestingUserId.Value);
            }
        }

        return shelter;
    }
}