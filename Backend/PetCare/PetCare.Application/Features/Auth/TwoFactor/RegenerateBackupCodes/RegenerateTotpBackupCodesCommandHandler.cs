namespace PetCare.Application.Features.Auth.TwoFactor.RegenerateBackupCodes;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using System;
using System.Threading.Tasks;

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
            return new GetTotpBackupCodesResponseDto(false, "Користувач не авторизований.", Array.Empty<string>());
        }

        var codes = await this.userService.RegenerateTotpBackupCodesAsync(user); // новий метод сервісу
        this.logger.LogInformation("TOTP backup codes regenerated for user {Email}", user.Email);

        return new GetTotpBackupCodesResponseDto(true, "Резервні коди перегенеровано успішно.", codes);
    }
}
