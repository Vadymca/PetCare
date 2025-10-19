namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.SpecieDtos;
using PetCare.Domain.Aggregates;
using PetCare.Domain.Entities;

/// <summary>
/// AutoMapper profile for mapping Specie aggregate to DTOs.
/// </summary>
public sealed class SpecieProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpecieProfile"/> class and configures object mappings between Specie and.
    /// SpecieListDto types.
    /// </summary>
    /// <remarks>This constructor sets up the mapping rules used by AutoMapper to convert Specie domain
    /// entities to SpecieListDto data transfer objects. The mapping ensures that the Id and Name properties are
    /// correctly transferred. This configuration is typically required for object-to-object mapping in data access or
    /// API layers.</remarks>
    public SpecieProfile()
    {
        // Mapping from Specie aggregate to SpecieListDto
        this.CreateMap<Specie, SpecieListDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

        this.CreateMap<Specie, SpecieDetailDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name.Value));

        this.CreateMap<Breed, BreedListDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name.Value))
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description));

        this.CreateMap<Specie, SpecieBriefDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name.Value));

        this.CreateMap<Breed, BreedWithSpecieDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name.Value))
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description))
            .ForCtorParam("Specie", opt => opt.MapFrom(src => src.Specie));
    }
}
