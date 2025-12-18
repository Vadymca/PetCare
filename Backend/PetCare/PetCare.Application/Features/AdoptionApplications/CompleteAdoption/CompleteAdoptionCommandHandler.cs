namespace PetCare.Application.Features.AdoptionApplications.CompleteAdoption;

using MediatR;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the completion of the adoption process.
/// </summary>
public sealed class CompleteAdoptionCommandHandler : IRequestHandler<CompleteAdoptionCommand, Unit>
{
    private readonly IAdoptionApplicationService adoptionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompleteAdoptionCommandHandler"/> class.
    /// </summary>
    /// <param name="adoptionService">The service responsible for adoption application operations. </param>
    public CompleteAdoptionCommandHandler(IAdoptionApplicationService adoptionService)
    {
        this.adoptionService = adoptionService;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(CompleteAdoptionCommand request, CancellationToken cancellationToken)
    {
        await this.adoptionService.CompleteAdoptionAsync(request.ApplicationId, request.IsAdopted, cancellationToken);
        return Unit.Value;
    }
}
