namespace PetCare.Application.Features.Species.DeleteSpecie;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for processing <see cref="DeleteSpecieCommand"/>.
/// </summary>
public sealed class DeleteSpecieCommandHandler : IRequestHandler<DeleteSpecieCommand, DeleteSpecieResponseDto>
{
    private readonly ISpecieService specieService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSpecieCommandHandler"/> class.
    /// </summary>
    /// <param name="specieService">The specie service.</param>
    public DeleteSpecieCommandHandler(ISpecieService specieService)
    {
        this.specieService = specieService ?? throw new ArgumentNullException(nameof(specieService));
    }

    /// <inheritdoc />
    public async Task<DeleteSpecieResponseDto> Handle(DeleteSpecieCommand request, CancellationToken cancellationToken)
    {
        await this.specieService.DeleteSpeciesAsync(request.Id, cancellationToken);
        return new DeleteSpecieResponseDto(true, $"Вид із Id '{request.Id}' успішно видалено.");
    }
}
