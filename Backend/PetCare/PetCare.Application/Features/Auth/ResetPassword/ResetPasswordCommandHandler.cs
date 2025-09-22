namespace PetCare.Application.Features.Auth.ResetPassword;

using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;

/// <summary>
/// Handles resetting user's password using token from email.
/// </summary>
public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponseDto>
{
    private readonly IUserService userService;
    private readonly ILogger<ResetPasswordCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResetPasswordCommandHandler"/> class.
    /// </summary>
    /// <param name="userService">The user service responsible for user-related operations.</param>
    /// <param name="logger">The logger instance used for logging information and warnings.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="userService"/> or <paramref name="logger"/> is <c>null</c>.
    /// </exception>
    public ResetPasswordCommandHandler(
        IUserService userService,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the password reset request by verifying the user and resetting the password
    /// if the provided token is valid.
    /// </summary>
    /// <param name="request">The <see cref="ResetPasswordCommand"/> containing email, token, and new password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> containing a <see cref="ResetPasswordResponseDto"/>
    /// that indicates whether the operation succeeded and an appropriate message.
    /// </returns>
    public async Task<ResetPasswordResponseDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindByEmailAsync(request.Email);
        if (user is null)
        {
            this.logger.LogWarning("ResetPassword requested for non-existing email: {Email}", request.Email);

            // Не повідомляємо, що користувач не існує
            return new ResetPasswordResponseDto(
                Success: true,
                Message: "Якщо користувач існує, пароль було скинуто.");
        }

        var success = await this.userService.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!success)
        {
            throw new InvalidOperationException("Не вдалося скинути пароль: Invalid token або пароль не відповідає вимогам.");
        }

        return new ResetPasswordResponseDto(
            Success: true,
            Message: "Пароль успішно змінено.");
    }
}
