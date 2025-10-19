namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Domain.Aggregates;

/// <summary>
/// Provides AutoMapper configuration for mapping shelter list domain models to data transfer objects (DTOs).
/// </summary>
/// <remarks>This profile defines mapping rules used by AutoMapper to transform shelter-related entities for list
/// operations. Register this profile with the AutoMapper configuration to enable shelter list mappings throughout the
/// application.</remarks>
public sealed class ShelterListProfile : Profile
{
    public ShelterListProfile()
    {
        this.CreateMap<Shelter, ShelterListDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Value))
            .ForMember(dest => dest.ContactPhone, opt => opt.MapFrom(src => src.ContactPhone.Value))
            .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail.Value))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.CurrentOccupancy, opt => opt.MapFrom(src => src.CurrentOccupancy))
            .ForMember(dest => dest.HasFreeCapacity, opt => opt.MapFrom(src => src.CurrentOccupancy < src.Capacity))
            .ForMember(dest => dest.VirtualTourUrl, opt => opt.MapFrom(src => src.VirtualTourUrl))
            .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => src.WorkingHours))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
            .ForMember(dest => dest.SocialMedia, opt => opt.MapFrom(src => src.SocialMedia))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ReverseMap();

    }
}
