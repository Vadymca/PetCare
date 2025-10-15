namespace PetCare.Application.Features.Auth.TwoFactor.RegenerateBackupCodes;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles regeneration of TOTP backup codes for the current user.
/// </summary>
public sealed class RegenerateTotpBackupCodesCommandHandler
    : IRequestHandler<RegenerateTotpBackupCodesCommand, GetTotpBackupCodesResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<RegenerateTotpBackupCodesCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegenerateTotpBackupCodesCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to retrieve and manage user data.</param>
    /// <param name="logger">The logger instance for logging operational messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="userService"/> or <paramref name="logger"/> is null.</exception>
    public RegenerateTotpBackupCodesCommandHandler(
        IUserService userService,
        ILogger<RegenerateTotpBackupCodesCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the <see cref="RegenerateTotpBackupCodesCommand"/> request by regenerating
    /// TOTP backup codes for the current user.
    /// </summary>
    /// <param name="request">The command to regenerate backup codes.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>
    /// A <see cref="GetTotpBackupCodesResponseDto"/> containing the success status, message,
    /// and the regenerated backup codes (if successful).
    /// </returns>
    public async Task<GetTotpBackupCodesResponseDto> Handle(
        RegenerateTotpBackupCodesCommand request,
        CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to regenerate TOTP backup codes.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        var codes = await this.userService.RegenerateTotpBackupCodesAsync(user);
        this.logger.LogInformation("TOTP backup codes regenerated for user {Email}", user.Email);

        return new GetTotpBackupCodesResponseDto(
            Success: true,
            Message: "Резервні коди перегенеровано успішно.",
            BackupCodes: codes);
    }
}
