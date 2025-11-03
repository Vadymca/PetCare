namespace PetCare.Application.Features.Users.UpdateUser;

using System.Threading;
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
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly IZipcodebaseService zipcodebaseService;
    private readonly IStorageService storageService;
    private readonly ILogger<UpdateUserCommandHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class with the specified user service, object mapper,.
    /// and logger.
    /// </summary>
    /// <param name="userService">The service used to perform user-related operations. Cannot be null.</param>
    /// <param name="mapper">The mapper used to convert between domain and data transfer objects. Cannot be null.</param>
    /// <param name="zipcodebaseService">Service to resolve addresses by postal code.</param>
    /// <param name="storageService">File storage service for handling profile photos.</param>
    /// <param name="logger">The logger used to record diagnostic and operational information for this handler. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="userService"/>, <paramref name="mapper"/>, or <paramref name="logger"/> is null.</exception>
    public UpdateUserCommandHandler(
        IUserService userService,
        IMapper mapper,
        IZipcodebaseService zipcodebaseService,
        IStorageService storageService,
        ILogger<UpdateUserCommandHandler> logger)
    {
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.zipcodebaseService = zipcodebaseService ?? throw new ArgumentNullException(nameof(zipcodebaseService));
        this.storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userService.FindByIdAsync(request.Id)
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

        await this.userService.UpdateUserAsync(user, cancellationToken);

        if (!string.IsNullOrWhiteSpace(oldAvatarUrl))
        {
            try
            {
                var objectName = Path.GetFileName(oldAvatarUrl);
                await this.storageService.DeleteFileAsync(objectName);
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

        var updated = await this.userService.FindByIdAsync(request.Id);

        var userDto = this.mapper.Map<UserDto>(updated);
        var roles = await this.userService.GetRolesAsync(updated!);
        userDto = userDto with { Role = roles.FirstOrDefault() ?? "User" };

        this.logger.LogInformation("User {UserId} updated by admin", request.Id);

        return userDto;
    }
}