namespace PetCare.Application.Features.Auth.Register;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly UserManager<User> userManager;
    private readonly ILogger<RegisterUserCommandHandler> logger;

    public RegisterUserCommandHandler(UserManager<User> userManager, ILogger<RegisterUserCommandHandler> logger)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Спроба реєстрації користувача з email {Email}", request.email);

        // Валідуємо вхідні дані
        if (string.IsNullOrWhiteSpace(request.email))
            throw new ArgumentException("Email не може бути порожнім.", nameof(request.email));
        if (string.IsNullOrWhiteSpace(request.phoneNumber))
            throw new ArgumentException("Телефон не може бути порожнім.", nameof(request.phoneNumber));
        if (string.IsNullOrWhiteSpace(request.firstName))
            throw new ArgumentException("Ім'я не може бути порожнім.", nameof(request.firstName));
        if (string.IsNullOrWhiteSpace(request.lastName))
            throw new ArgumentException("Прізвище не може бути порожнім.", nameof(request.lastName));
        if (string.IsNullOrWhiteSpace(request.password))
            throw new ArgumentException("Пароль не може бути порожнім.", nameof(request.password));

        // Формуємо username з email
        var userName = request.email.Replace("@", "_").Replace(".", "_");
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Username некоректний.", nameof(userName));

        // Перевіряємо унікальність email
        var existingUser = await this.userManager.FindByEmailAsync(request.email);
        if (existingUser != null)
        {
            this.logger.LogWarning("Користувач із email {Email} уже існує.", request.email);
            throw new InvalidOperationException($"Користувач із email {request.email} уже зареєстрований.");
        }

        // Створюємо користувача через User.Create
        var user = User.Create(
            request.email,
            string.Empty, // passwordHash не потрібен, бо UserManager хешує пароль
            request.firstName,
            request.lastName,
            request.phoneNumber,
            UserRole.User);

        // Встановлюємо UserName і NormalizedUserName для Identity
        user.UserName = userName;
        user.NormalizedUserName = userName.ToUpperInvariant();
        user.NormalizedEmail = request.email.ToUpperInvariant(); // Identity потребує NormalizedEmail

        // Створюємо користувача через UserManager
        var result = await this.userManager.CreateAsync(user, request.password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            this.logger.LogError("Помилка створення користувача: {Errors}", errors);
            throw new InvalidOperationException($"Не вдалося створити користувача: {errors}");
        }

        // Додаємо роль
        var roleResult = await this.userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            this.logger.LogError("Помилка додавання ролі: {Errors}", roleErrors);
            throw new InvalidOperationException($"Не вдалося додати роль: {roleErrors}");
        }

        this.logger.LogInformation("Користувач {Email} успішно зареєстрований з ID {UserId}", request.email, user.Id);

        // Повертаємо DTO
        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Phone,
            "User");
    }
}