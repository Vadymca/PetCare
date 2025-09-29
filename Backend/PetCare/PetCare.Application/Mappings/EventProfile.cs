namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.EventDtos;
using PetCare.Domain.Entities;

/// <summary>
/// AutoMapper profile for mapping <see cref="Event"/> to <see cref="EventDto"/>.
/// </summary>
public sealed class EventProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventProfile"/> class.
    /// Configures mappings from <see cref="Event"/> to <see cref="EventDto"/>.
    /// </summary>
    public EventProfile()
    {
        this.CreateMap<Event, EventDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ShelterId, opt => opt.MapFrom(src => src.ShelterId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? src.Address.ToString() : null))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
