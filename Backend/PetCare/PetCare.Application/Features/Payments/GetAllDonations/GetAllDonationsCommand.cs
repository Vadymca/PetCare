namespace PetCare.Application.Features.Payments.GetAllDonations;

using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a request to retrieve all donations as a read-only list of donation summary data transfer objects.
/// </summary>
public sealed record GetAllDonationsCommand() : IRequest<IReadOnlyList<DonationListDto>>;
