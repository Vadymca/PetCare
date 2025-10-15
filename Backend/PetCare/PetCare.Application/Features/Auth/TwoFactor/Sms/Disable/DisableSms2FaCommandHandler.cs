namespace PetCare.Application.Features.Auth.TwoFactor.Sms.Disable;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the disabling of SMS 2FA for the current user.
/// </summary>
public sealed class DisableSms2FaCommandHandler : IRequestHandler<DisableSms2FaCommand, DisableSms2FaResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<DisableSms2FaCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisableSms2FaCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">Service for user management operations.</param>
    /// <param name="logger">Logger for handling operational logs.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="userService"/> or <paramref name="logger"/> is null.</exception>
    public DisableSms2FaCommandHandler(
        IUserService userService,
        ILogger<DisableSms2FaCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<DisableSms2FaResponseDto> Handle(DisableSms2FaCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.GetCurrentUserAsync();
        if (user == null)
        {
            this.logger.LogWarning("Unauthorized attempt to disable SMS 2FA.");
            throw new UnauthorizedAccessException("Користувач не авторизований.");
        }

        if (!user.PhoneNumberConfirmed)
        {
            throw new InvalidOperationException("SMS 2FA вже відключено або номер телефону не підтверджений.");
        }

        await this.userService.DisableSms2FaAsync(user);

        this.logger.LogInformation("SMS 2FA disabled for user {UserId}", user.Id);
        return new DisableSms2FaResponseDto(true, "SMS 2FA успішно відключено.");
    }
}
