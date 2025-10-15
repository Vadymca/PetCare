namespace PetCare.Application.Features.Auth.TwoFactor.RecoveryCodes;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the retrieval of TOTP backup recovery codes.
/// </summary>
public sealed class GetRecoveryCodesQueryHandler
    : IRequestHandler<GetRecoveryCodesCommand, RecoveryCodesResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<GetRecoveryCodesQueryHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetRecoveryCodesQueryHandler"/> class.
    /// </summary>
    /// <param name="userService">Service for retrieving and managing the current user.</param>
    /// <param name="logger">Logger instance for diagnostic and error logging.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="userService"/> or <paramref name="logger"/> is <c>null</c>.
    /// </exception>
    public GetRecoveryCodesQueryHandler(
        IUserService userService,
        ILogger<GetRecoveryCodesQueryHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<RecoveryCodesResponseDto> Handle(
        GetRecoveryCodesCommand request,
        CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to get recovery codes.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        var codes = await this.userService.GetTotpBackupCodesAsync(user);
        this.logger.LogInformation("Generated {Count} recovery codes for user {UserId}", codes.Count, user.Id);

        return new RecoveryCodesResponseDto(
             Success: true,
             Message: "Коди відновлення успішно згенеровані.",
             Codes: codes);
    }
}
