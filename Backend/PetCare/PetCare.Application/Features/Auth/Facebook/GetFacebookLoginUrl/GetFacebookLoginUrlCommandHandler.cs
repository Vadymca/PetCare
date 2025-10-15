namespace PetCare.Application.Features.Auth.Facebook.GetFacebookLoginUrl;

using System;
using System.Threading.Tasks;
using MediatR;
using PetCare.Application.Interfaces;

/// <summary>
/// Handler for the <see cref="GetFacebookLoginUrlCommand"/>.
/// </summary>
public sealed class GetFacebookLoginUrlCommandHandler : IRequestHandler<GetFacebookLoginUrlCommand, string>
{
    private readonly IFacebookAuthService facebookAuthService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFacebookLoginUrlCommandHandler"/> class.
    /// </summary>
    /// <param name="facebookAuthService">The Facebook authentication service.</param>
    public GetFacebookLoginUrlCommandHandler(IFacebookAuthService facebookAuthService)
    {
        this.facebookAuthService = facebookAuthService ?? throw new ArgumentNullException(nameof(facebookAuthService));
    }

    /// <inheritdoc/>
    public Task<string> Handle(GetFacebookLoginUrlCommand request, CancellationToken cancellationToken)
    {
        var loginUrl = this.facebookAuthService.GetLoginUrl(request.State);
        return Task.FromResult(loginUrl);
    }
}
