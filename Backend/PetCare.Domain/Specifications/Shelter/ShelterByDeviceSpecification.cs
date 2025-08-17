namespace PetCare.Domain.Specifications.Shelter;
using PetCare.Domain.Aggregates;
using System;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// Specification for filtering shelters by device ID.
/// </summary>
public sealed class ShelterByDeviceSpecification : Specification<Shelter>
{
    private readonly Guid deviceId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShelterByDeviceSpecification"/> class.
    /// </summary>
    /// <param name="deviceId">The device ID to filter shelters by.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="deviceId"/> is empty.</exception>
    public ShelterByDeviceSpecification(Guid deviceId)
    {
        if (deviceId == Guid.Empty)
        {
            throw new ArgumentException("Ідентифікатор пристрою не може бути порожнім.", nameof(deviceId));
        }

        this.deviceId = deviceId;
    }

    /// <inheritdoc />
    public override Expression<Func<Shelter, bool>> ToExpression()
    {
        return s => s.IoTDevices.Any(d => d.Id == this.deviceId);
    }
}
