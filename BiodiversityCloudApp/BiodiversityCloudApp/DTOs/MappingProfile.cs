using System.Runtime.CompilerServices;
using AutoMapper;
using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                .ForMember(u => u.PasswordHash, opt => opt.Ignore());

            CreateMap<Photo, PhotoDto>()
                .ForMember(dest => dest.Url, opt=> opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Comment, CommentDto>();
            CreateMap<Animal, AnimalDto>()
                .ForMember(dest => dest.MicroObservations, opt => opt.MapFrom(src => src.MicroObservations));

            CreateMap<MicroObservation, MicroObservationDto>()
                .ForMember(dest => dest.AnimalId, opt => opt.MapFrom(src => src.AnimalId));

            CreateMap<Observation, ObservationDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ObserverName, opt => opt.MapFrom(src => src.User.Name))  
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))  
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))  
                .ForMember(dest => dest.Temperature, opt => opt.MapFrom(src => src.EnvironmentalConditions.Temperature))
                .ForMember(dest => dest.Humidity, opt => opt.MapFrom(src => src.EnvironmentalConditions.Humidity))
                .ForMember(dest => dest.Weather, opt => opt.MapFrom(src => src.EnvironmentalConditions.Weather))
                .ForMember(dest => dest.AdditionalDetails, opt => opt.MapFrom(src => src.EnvironmentalConditions.AdditionalDetails))
                .ForMember(dest => dest.MicroObservations, opt => opt.MapFrom(src => src.MicroObservations));  
            CreateMap<ObservationDto, Observation>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.EnvironmentalConditions, opt => opt.MapFrom(src => new EnvironmentalConditions
                {
                    Temperature = src.Temperature,
                    Humidity = src.Humidity,
                    Weather = src.Weather,
                    AdditionalDetails = src.AdditionalDetails
                }));

            // Map EnvironmentalConditions from DTO to Entity for ObservationDto to Observation
            CreateMap<ObservationDto, Observation>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.EnvironmentalConditions, opt => opt.MapFrom(src => new EnvironmentalConditions
                {
                    Temperature = src.Temperature,
                    Humidity = src.Humidity,
                    Weather = src.Weather,
                    AdditionalDetails = src.AdditionalDetails
                }));

            // Mapping for Photo and Comment (additional mappings for nested DTOs)
            CreateMap<PhotoDto, Photo>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));  // Assuming Photo has Url and Description

            CreateMap<CommentDto, Comment>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text));  // Assuming Comment has Text

            // Map MicroObservationDto to MicroObservation with Animal excluded (assuming Animal is not part of incoming data)
            CreateMap<MicroObservationDto, MicroObservation>()
                .ForMember(dest => dest.Animal, opt => opt.Ignore());  // Animal is excluded in this mapping

            // Mapping AnimalDto to Animal with MicroObservations excluded (assuming it's not part of incoming data)
            CreateMap<AnimalDto, Animal>()
                .ForMember(dest => dest.MicroObservations, opt => opt.Ignore());  // MicroObservations are excluded here
        }
    }
}