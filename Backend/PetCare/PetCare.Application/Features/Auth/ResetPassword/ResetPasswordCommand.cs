using MediatR;
using PetCare.Application.Dtos;

namespace PetCare.Application.Features.Auth.ResetPassword
{
    public sealed record ResetPasswordCommand(
     string Email,
     string Token,
     string NewPassword)
        : IRequest<ResetPasswordResponseDto>;
}
