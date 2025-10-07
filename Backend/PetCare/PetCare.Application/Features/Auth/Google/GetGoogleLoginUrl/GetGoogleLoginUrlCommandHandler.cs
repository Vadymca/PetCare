namespace PetCare.Application.Features.Auth.Google.GetGoogleLoginUrl;

using MediatR;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

/// <summary>
/// Handler for the <see cref="GetGoogleLoginUrlCommand"/>.
/// </summary>
public sealed class GetGoogleLoginUrlCommandHandler : IRequestHandler<GetGoogleLoginUrlCommand, string>
{
    private readonly IGoogleAuthService googleAuthService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGoogleLoginUrlCommandHandler"/> class.
    /// </summary>
    /// <param name="googleAuthService">The Google authentication service.</param>
    public GetGoogleLoginUrlCommandHandler(IGoogleAuthService googleAuthService)
    {
        this.googleAuthService = googleAuthService ?? throw new ArgumentNullException(nameof(googleAuthService));
    }

    /// <inheritdoc/>
    public Task<string> Handle(GetGoogleLoginUrlCommand request, CancellationToken cancellationToken)
    {
        var loginUrl = this.googleAuthService.GetLoginUrl(request.State);
        return Task.FromResult(loginUrl);
    }
}
