namespace PetCare.Application.Features.Auth.ConfirmEmail;

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles the <see cref="ConfirmEmailCommand"/> to confirm a user's email address.
/// </summary>
public sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ConfirmEmailResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<ConfirmEmailCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmEmailCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service used to query and manage users.</param>
    /// <param name="logger">The logger instance used to record diagnostic and operational messages.</param>
    public ConfirmEmailCommandHandler(
        IUserService userService,
        ILogger<ConfirmEmailCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the <see cref="ConfirmEmailCommand"/> request to confirm a user's email.
    /// </summary>
    /// <param name="request">The command containing the user's email and confirmation token.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation,
    /// with <c>true</c> if the email was successfully confirmed, otherwise <c>false</c>.
    /// </returns>
    public async Task<ConfirmEmailResponseDto> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindByEmailAsync(request.Email);
        if (user == null)
        {
            this.logger.LogWarning("Attempt to confirm email for non-existent user: {Email}", request.Email);
            throw new InvalidOperationException("Користувач із вказаною електронною поштою не знайдений.");
        }

        if (user.EmailConfirmed)
        {
            this.logger.LogInformation("Email already confirmed for user: {Email}", request.Email);
            throw new InvalidOperationException("Електронна пошта вже підтверджена.");
        }

        var confirmed = await this.userService.ConfirmEmailAsync(user, request.Token);
        if (!confirmed)
        {
            this.logger.LogWarning("Invalid or expired token for user: {Email}", request.Email);
            throw new InvalidOperationException("Невірний або прострочений токен підтвердження.");
        }

        this.logger.LogInformation("Email successfully confirmed for user: {Email}", request.Email);

        return new ConfirmEmailResponseDto(
            Success: true,
            Message: "Електронна пошта успішно підтверджена.");
    }
}
