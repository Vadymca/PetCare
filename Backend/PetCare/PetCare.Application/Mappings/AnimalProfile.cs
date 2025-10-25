namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

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
        // Nested entities
        this.CreateMap<Specie, SpecieDto>();
        this.CreateMap<Breed, BreedDto>();
        this.CreateMap<Shelter, ShelterInfoDto>();

        // Main mapping
        this.CreateMap<Animal, AnimalDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday != null
                               ? src.Birthday.ToString()
                               : null))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.HealthConditions, opt => opt.MapFrom(src => src.HealthConditions))
            .ForMember(dest => dest.SpecialNeeds, opt => opt.MapFrom(src => src.SpecialNeeds))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size.ToString()))
            .ForMember(dest => dest.Temperaments, opt => opt.MapFrom(src => src.Temperaments.ToString()))
            .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CareCost, opt => opt.MapFrom(src => src.CareCost))
            .ForMember(dest => dest.AdoptionRequirements, opt => opt.MapFrom(src => src.AdoptionRequirements))
            .ForMember(dest => dest.MicrochipId, opt => opt.MapFrom(src => src.MicrochipId != null
            ? src.MicrochipId.Value : null))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.IsSterilized, opt => opt.MapFrom(src => src.IsSterilized))
            .ForMember(dest => dest.IsUnderCare, opt => opt.MapFrom(src => src.IsUnderCare))
            .ForMember(dest => dest.HaveDocuments, opt => opt.MapFrom(src => src.HaveDocuments))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForCtorParam("Species", opt => opt.MapFrom(src => src.Breed!.Specie))
            .ForMember(dest => dest.Breed, opt => opt.MapFrom(src => src.Breed!))
            .ForMember(dest => dest.Shelter, opt => opt.MapFrom(src => src.Shelter!));
    }
}
