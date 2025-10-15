namespace PetCare.Application.Features.Users.UpdateUser;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Application.Interfaces;
using PetCare.Domain.ValueObjects;

/// <summary>
/// Handles UpdateUserCommand — admin updates an existing user.
/// All business errors are thrown as exceptions (handled by ExceptionHandlingMiddleware).
/// </summary>
public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IZipcodebaseService zipcodebaseService;
    private readonly IFileStorageService fileStorage;
    private readonly ILogger<UpdateUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="userRepository">Repository for user aggregate.</param>
    /// <param name="mapper">AutoMapper instance.</param>
    /// <param name="userService">User service (for roles, password).</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="zipcodebaseService">Service to resolve addresses by postal code.</param>
    /// <param name="fileStorage">File storage service for handling profile photos.</param>
    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IUserService userService,
        IZipcodebaseService zipcodebaseService,
        IFileStorageService fileStorage,
        ILogger<UpdateUserCommandHandler> logger)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.zipcodebaseService = zipcodebaseService ?? throw new ArgumentNullException(nameof(zipcodebaseService));
        this.fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the <see cref="UpdateUserCommand"/>.
    /// Updates user profile, credentials, role, preferences, points, and resolves address if postal code is provided.
    /// </summary>
    /// <param name="request">Update user command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated <see cref="UserDto"/>.</returns>
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Користувача не знайдено.");

        string? oldAvatarUrl = user.ProfilePhoto;
        string? oldPhone = user.Phone;

        user.UpdateProfile(
            firstName: request.FirstName,
            lastName: request.LastName,
            phone: request.Phone,
            profilePhoto: request.ProfilePhoto,
            language: request.Language,
            postalCode: request.PostalCode);

        // Якщо телефон змінився — скидаємо підтвердження
        if (!string.IsNullOrWhiteSpace(request.Phone) && request.Phone != oldPhone)
        {
            user.PhoneNumberConfirmed = false;
            this.logger.LogInformation("Phone number updated for user {UserId}, PhoneNumberConfirmed reset to false", request.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
        {
            user.Email = request.Email;
        }

        if (request.Preferences != null)
        {
            user.UpdatePreferences(request.Preferences);
        }

        if (request.Points.HasValue && request.Points.Value != user.Points)
        {
            var userRoles = await this.userService.GetRolesAsync(user);
            if (userRoles.Contains("Admin"))
            {
                var difference = request.Points.Value - user.Points;

                if (difference > 0)
                {
                    user.AddPoints(difference, request.Id);
                }
                else
                {
                    user.DeductPoints(-difference, request.Id);
                }
            }
            else
            {
                this.logger.LogWarning("User {UserId} tried to update points without Admin role", request.Id);
                throw new InvalidOperationException("Ви не можете змінювати свої бали. Тільки адміністратор може це робити.");
            }
        }

        if (!string.IsNullOrWhiteSpace(request.PostalCode))
        {
            try
            {
                var address = await this.zipcodebaseService.ResolveAddressAsync(request.PostalCode, cancellationToken);
                if (address != null)
                {
                    user.UpdateAddress(address);
                    this.logger.LogInformation(
                        "Address resolved for postal code {PostalCode}: {Address}",
                        request.PostalCode,
                        address.Value);
                }
                else
                {
                    user.UpdateAddress(Address.Unknown());
                    this.logger.LogWarning(
                        "Could not resolve address for postal code {PostalCode}. Default address set: {DefaultAddress}",
                        request.PostalCode,
                        Address.Unknown().Value);
                }
            }
            catch (Exception ex)
            {
                user.UpdateAddress(Address.Unknown());
                this.logger.LogWarning(
                    ex,
                    "Не вдалося згенерувати адресу для postal code {PostalCode}. Default address set: {DefaultAddress}",
                    request.PostalCode,
                    Address.Unknown().Value);
            }
        }

        await this.userRepository.UpdateAsync(user, cancellationToken);

        if (!string.IsNullOrWhiteSpace(oldAvatarUrl))
        {
            try
            {
                await this.fileStorage.DeleteAsync(oldAvatarUrl);
                this.logger.LogInformation("Old avatar {OldAvatar} deleted successfully", oldAvatarUrl);
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Failed to delete old avatar {OldAvatar}", oldAvatarUrl);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            await this.userService.ChangePasswordAsync(user.Id, request.Password, cancellationToken);
        }

        var updated = await this.userRepository.GetByIdAsync(request.Id, cancellationToken);

        var userDto = this.mapper.Map<UserDto>(updated);
        var roles = await this.userService.GetRolesAsync(updated!);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        this.logger.LogInformation("User {UserId} updated by admin", request.Id);

        return userDto;
    }
}
