namespace PetCare.Application.Mappings;

using AutoMapper;
using PetCare.Application.Dtos.AnimalAidRequestDtos;
using PetCare.Application.Dtos.AnimalDtos;
using PetCare.Domain.Entities;

/// <summary>
/// AutoMapper profile for AnimalAidRequest mappings.
/// </summary>
public sealed class AnimalAidRequestProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnimalAidRequestProfile"/> class.
    /// </summary>
    public AnimalAidRequestProfile()
    {
        this.CreateMap<AnimalAidRequest, AnimalAidRequestListDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Slug", opt => opt.MapFrom(src => src.Slug.Value))
            .ForCtorParam("Shelter", opt => opt.MapFrom(src => src.Shelter != null
                ? new ShelterInfoDto(src.Shelter.Id, src.Shelter.Name.Value, src.Shelter.Slug.Value)
                : null))
            .ForCtorParam("Title", opt => opt.MapFrom(src => src.Title.Value))
            .ForCtorParam("ShortDescription", opt => opt.MapFrom(src => src.ShortDescription ?? string.Empty))
            .ForCtorParam("Category", opt => opt.MapFrom(src => src.Category))
            .ForCtorParam("AllreadyDonated", opt => opt.MapFrom(src => src.Donations.Sum(d => d.Donation != null ? d.Donation.Amount : 0)))
            .ForCtorParam("EstimatedCost", opt => opt.MapFrom(src => src.EstimatedCost ?? 0m))
            .ForCtorParam("Status", opt => opt.MapFrom(src => src.Status))
            .ForCtorParam("Photo", opt => opt.MapFrom(src => src.Photos.FirstOrDefault()));

        this.CreateMap<AnimalAidRequest, AnimalAidRequestDetailsDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("Slug", opt => opt.MapFrom(src => src.Slug.Value))
            .ForCtorParam("Shelter", opt => opt.MapFrom(src => src.Shelter != null
                ? new ShelterInfoDto(src.Shelter.Id, src.Shelter.Name.Value, src.Shelter.Slug.Value)
                : null))
            .ForCtorParam("Title", opt => opt.MapFrom(src => src.Title.Value))
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description ?? string.Empty))
            .ForCtorParam("Category", opt => opt.MapFrom(src => src.Category))
            .ForCtorParam("EstimatedCost", opt => opt.MapFrom(src => src.EstimatedCost ?? 0m))
            .ForCtorParam("AllreadyDonated", opt => opt.MapFrom(src => src.Donations.Sum(d => d.Donation != null ? d.Donation.Amount : 0)))
            .ForCtorParam("Status", opt => opt.MapFrom(src => src.Status))
            .ForCtorParam("Photos", opt => opt.MapFrom(src => src.Photos))
            .ForCtorParam("CreatedAt", opt => opt.MapFrom(src => src.CreatedAt));

        this.CreateMap<AnimalAidRequest, UrgentAnimalAidRequestDto>()
            .ForMember(dest => dest.CollectedAmount, opt => opt.MapFrom(src => src.Donations.Sum(d => d.Donation != null ? d.Donation.Amount : 0)))
            .ForMember(dest => dest.DonationsCount, opt => opt.MapFrom(src => src.Donations.Count))
            .ForMember(dest => dest.Donations, opt => opt.MapFrom(src => src.Donations));
    }
}
