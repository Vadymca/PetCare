namespace PetCare.Application.Dtos.UserDtos;

/// <summary>
/// Represents the result of an operation to add a user role, including success status and an associated message.
/// </summary>
/// <param name="Success">A value indicating whether the user role was added successfully. Set to <see langword="true"/> if the operation
/// succeeded; otherwise, <see langword="false"/>.</param>
/// <param name="Message">A message providing additional information about the result of the add user role operation. May contain error
/// details or confirmation text.</param>
public sealed record AddUserRoleResponseDto(
 bool Success,
 string Message);
