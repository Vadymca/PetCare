namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.ShelterDtos;
using PetCare.Domain.Aggregates;

/// <summary>
/// AutoMapper profile for mapping <see cref="Shelter"/> entities to <see cref="ShelterDto"/> objects.
/// </summary>
public sealed class ShelterProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShelterProfile"/> class.
    /// Configures mappings from <see cref="Shelter"/> to <see cref="ShelterDto"/>.
    /// </summary>
    public ShelterProfile()
    {
        this.CreateMap<Shelter, ShelterDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.ToString()))
            .ForMember(dest => dest.ContactPhone, opt => opt.MapFrom(src => src.ContactPhone.Value))
            .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(dest => dest.CurrentOccupancy, opt => opt.MapFrom(src => src.CurrentOccupancy))
            .ForMember(dest => dest.VirtualTourUrl, opt => opt.MapFrom(src => src.VirtualTourUrl))
            .ForMember(dest => dest.WorkingHours, opt => opt.MapFrom(src => src.WorkingHours));
    }
}
