namespace PetCare.Application.Features.Shelters.GetShelterById;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles <see cref="GetShelterByIdCommand"/> requests.
/// </summary>
public sealed class GetShelterByIdCommandHandler : IRequestHandler<GetShelterByIdCommand, ShelterDto?>
{
    private readonly IShelterService shelterService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetShelterByIdCommandHandler"/> class with the specified shelter service and.
    /// object mapper.
    /// </summary>
    /// <param name="shelterService">The service used to retrieve shelter information.</param>
    /// <param name="mapper">The mapper used to convert shelter data between domain and DTO representations.</param>
    /// <exception cref="ArgumentNullException">Thrown if shelterService or mapper is null.</exception>
    public GetShelterByIdCommandHandler(IShelterService shelterService, IMapper mapper)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<ShelterDto?> Handle(GetShelterByIdCommand request, CancellationToken cancellationToken)
    {
        if (request.Id == Guid.Empty)
        {
            throw new ArgumentException("Id не може бути порожнім.", nameof(request.Id));
        }

        var shelter = await this.shelterService.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Притулок не знайдено.");

        return this.mapper.Map<ShelterDto>(shelter);
    }
}
