namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Aggregates;
using System;

/// <summary>
/// AutoMapper profile for mapping <see cref="Animal"/> aggregate to <see cref="AnimalDto"/>.
/// </summary>
public sealed class AnimalProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalProfile"/> class.
    /// Configures mappings between <see cref="Animal"/> aggregate and <see cref="AnimalDto"/>.
    /// </summary>
    public AnimalProfile()
    {
        this.CreateMap<Animal, AnimalDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday != null
            ? (DateTime?)src.Birthday.Value.ToDateTime(TimeOnly.MinValue)
            : null))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.HealthStatus, opt => opt.MapFrom(src => src.HealthStatus))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
            .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.AdoptionRequirements, opt => opt.MapFrom(src => src.AdoptionRequirements))
            .ForMember(dest => dest.MicrochipId, opt => opt.MapFrom(src => src.MicrochipId != null
            ? src.MicrochipId.Value : null))
            .ForMember(dest => dest.IdNumber, opt => opt.MapFrom(src => src.IdNumber))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.IsSterilized, opt => opt.MapFrom(src => src.IsSterilized))
            .ForMember(dest => dest.HaveDocuments, opt => opt.MapFrom(src => src.HaveDocuments))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.BreedId, opt => opt.MapFrom(src => src.BreedId))
            .ForMember(dest => dest.BreedName, opt => opt.MapFrom(src => src.Breed != null
            ? src.Breed.Name.Value : null))
            .ForMember(dest => dest.ShelterId, opt => opt.MapFrom(src => src.ShelterId))
            .ForMember(dest => dest.ShelterName, opt => opt.MapFrom(src => src.Shelter != null
            ? src.Shelter.Name.Value : string.Empty));
    }
}
