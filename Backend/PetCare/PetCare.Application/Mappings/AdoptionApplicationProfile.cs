namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.AdoptionApplicationDtos;
using PetCare.Domain.Aggregates;

/// <summary>
/// AutoMapper profile for mapping <see cref="AdoptionApplication"/> to <see cref="AdoptionApplicationDto"/>.
/// </summary>
public sealed class AdoptionApplicationProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdoptionApplicationProfile"/> class.
    /// Configures mappings between <see cref="AdoptionApplication"/> and <see cref="AdoptionApplicationDto"/>.
    public AdoptionApplicationProfile()
    {
        this.CreateMap<AdoptionApplication, AdoptionApplicationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.AnimalId, opt => opt.MapFrom(src => src.AnimalId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => src.ApplicationDate))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.AdminNotes, opt => opt.MapFrom(src => src.AdminNotes))
            .ForMember(dest => dest.RejectionReason, opt => opt.MapFrom(src => src.RejectionReason))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy));
    }
}
