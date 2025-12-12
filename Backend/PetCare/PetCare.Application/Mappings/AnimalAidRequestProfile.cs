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
            .ForMember(dest => dest.Shelter, opt => opt.MapFrom(src => src.Shelter != null
                ? new ShelterInfoDto(src.Shelter.Id, src.Shelter.Name.Value, src.Shelter.Slug.Value)
                : null))
            .ForMember(
                       dest => dest.AllreadyDonated,
                       opt => opt.MapFrom(src => src.Donations.Sum(d => d.Donation != null ? d.Donation.Amount : 0)))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photos.FirstOrDefault()))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
            .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription ?? string.Empty));

        this.CreateMap<AnimalAidRequest, AnimalAidRequestDetailsDto>()
        .ForMember(dest => dest.Shelter, opt => opt.MapFrom(src => src.Shelter != null
            ? new ShelterInfoDto(src.Shelter.Id, src.Shelter.Name.Value, src.Shelter.Slug.Value)
            : null))
        .ForMember(dest => dest.AllreadyDonated, opt => opt.MapFrom(src => src.Donations.Sum(d => d.Donation != null ? d.Donation.Amount : 0)))
        .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
        .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        this.CreateMap<AnimalAidRequest, UrgentAnimalAidRequestDto>()
            .ForMember(dest => dest.CollectedAmount, opt => opt.MapFrom(src => src.Donations.Sum(d => d.Donation != null ? d.Donation.Amount : 0)))
            .ForMember(dest => dest.DonationsCount, opt => opt.MapFrom(src => src.Donations.Count))
            .ForMember(dest => dest.Donations, opt => opt.MapFrom(src => src.Donations));
    }
}
