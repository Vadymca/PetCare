namespace PetCare.Application.Features.Payments.GetProjectDonations;

using System;
using System.Collections.Generic;
using MediatR;
using PetCare.Application.Dtos.Payments;

/// <summary>
/// Represents a request to retrieve the list of donations associated with a specified project.
/// </summary>
/// <param name="ProjectId">The unique identifier of the project for which donations are to be retrieved.</param>
public sealed record GetProjectDonationsCommand(Guid ProjectId) : IRequest<IReadOnlyList<DonationListDto>>;
