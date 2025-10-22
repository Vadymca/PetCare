namespace PetCare.Application.Mappings;

using System.Linq;
using AutoMapper;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// AutoMapper profile for mapping Animal aggregates to AnimalListDto.
/// </summary>
public class AnimalListProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalListProfile"/> class.
    /// </summary>
    public AnimalListProfile()
    {
        // Mapping nested entities
        this.CreateMap<Specie, SpecieDto>();
        this.CreateMap<Breed, BreedDto>();
        this.CreateMap<Shelter, ShelterInfoDto>();

        // Main mapping: Animal → AnimalListDto
        this.CreateMap<Animal, AnimalListDto>()
             .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
             .ForCtorParam("Slug", opt => opt.MapFrom(src => src.Slug.Value))
             .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name.Value))
             .ForCtorParam("Photo", opt => opt.MapFrom(src => src.Photos.FirstOrDefault()))
             .ForCtorParam("Status", opt => opt.MapFrom(src => src.Status.ToString()))
             .ForCtorParam("Birthday", opt => opt.MapFrom(src => src.Birthday != null ? src.Birthday.ToString() : null))
             .ForCtorParam("Gender", opt => opt.MapFrom(src => src.Gender.ToString()))
             .ForCtorParam("IsUnderCare", opt => opt.MapFrom(src => src.IsUnderCare))
             .ForCtorParam("Species", opt => opt.MapFrom(src => src.Breed!.Specie))
             .ForCtorParam("Breed", opt => opt.MapFrom(src => src.Breed))
             .ForCtorParam("Shelter", opt => opt.MapFrom(src => src.Shelter));
    }
}
