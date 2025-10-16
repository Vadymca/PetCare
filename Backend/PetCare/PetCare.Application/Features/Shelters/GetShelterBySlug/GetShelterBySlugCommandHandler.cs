namespace PetCare.Application.Features.Shelters.GetShelterBySlug;

using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for processing <see cref="GetShelterBySlugCommand"/>.
/// </summary>
public sealed class GetShelterBySlugCommandHandler : IRequestHandler<GetShelterBySlugCommand, ShelterDto>
{
    private readonly IShelterService shelterService;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetShelterBySlugCommandHandler"/> class.
    /// </summary>
    /// <param name="shelterService">The shelter service instance.</param>
    /// <param name="mapper">The mapper instance.</param>
    public GetShelterBySlugCommandHandler(IShelterService shelterService, IMapper mapper)
    {
        this.shelterService = shelterService ?? throw new ArgumentNullException(nameof(shelterService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<ShelterDto> Handle(GetShelterBySlugCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Slug))
        {
            throw new ArgumentException("Slug не може бути порожнім.", nameof(request.Slug));
        }

        var shelter = await this.shelterService.GetBySlugAsync(request.Slug, cancellationToken)
                      ?? throw new InvalidOperationException($"Притулок зі slug '{request.Slug}' не знайдено.");

        return this.mapper.Map<ShelterDto>(shelter);
    }
}
