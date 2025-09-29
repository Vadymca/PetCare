namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.AuthDtos;
using PetCare.Domain.Aggregates;

/// <summary>
/// Defines mappings between <see cref="User"/> entity and <see cref="UserDto"/>.
/// </summary>
public sealed class UserProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfile"/> class.
    /// Configures mappings between <see cref="User"/> and <see cref="UserDto"/>.
    public UserProfile()
    {
        this.CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Email ?? string.Empty))
            .ForMember(
                dest => dest.Role,
                opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(
                dest => dest.Address,
                opt => opt.MapFrom(src => src.Address != null ? src.Address.ToString() : null))
            .ForMember(
                dest => dest.Preferences,
                opt => opt.MapFrom(src => src.Preferences))
            .ReverseMap()
            .ForMember(
                dest => dest.Role,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Address,
                opt => opt.Ignore());
    }
}
