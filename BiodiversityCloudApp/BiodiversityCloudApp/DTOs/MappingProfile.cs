using AutoMapper;
using BiodiversityCloudApp.DTOs.Common;
using BiodiversityCloudApp.DTOs.Observations;
using BiodiversityCloudApp.DTOs.ObservationRecords;
using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Common DTOs
            CreateMap<EnvironmentalConditionsDto, EnvironmentalConditions>()
                .ReverseMap();

            CreateMap<LocationDto, Location>()
                .ReverseMap();

            // Observation DTOs
            CreateMap<Observation, ObservationDto>();
            CreateMap<CreateObservationDto, Observation>();
            CreateMap<UpdateObservationDto, Observation>()
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartTime, opt => opt.Condition(src => src.StartTime != null))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.Condition(src => src.EndTime != null))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.ObservationStatus, opt => opt.Condition(src => src.ObservationStatus != null))
                .ForMember(dest => dest.ObservationStatus, opt => opt.MapFrom(src => src.ObservationStatus))
                .ForMember(dest => dest.StartLocation, opt => opt.Condition(src => src.StartLocation != null))
                .ForMember(dest => dest.StartLocation, opt => opt.MapFrom(src => src.StartLocation))
                .ForMember(dest => dest.EndLocation, opt => opt.Condition(src => src.EndLocation != null))
                .ForMember(dest => dest.EndLocation, opt => opt.MapFrom(src => src.EndLocation))
                .ForMember(dest => dest.ResearchType, opt => opt.Condition(src => src.ResearchType != null))
                .ForMember(dest => dest.ResearchType, opt => opt.MapFrom(src => src.ResearchType))
                .ForMember(dest => dest.PhenologicalPhase, opt => opt.Condition(src => src.PhenologicalPhase != null))
                .ForMember(dest => dest.PhenologicalPhase, opt => opt.MapFrom(src => src.PhenologicalPhase))
                .ForMember(dest => dest.EnvironmentalConditions, opt => opt.Condition(src => src.EnvironmentalConditions != null))
                .ForMember(dest => dest.EnvironmentalConditions, opt => opt.MapFrom(src => src.EnvironmentalConditions));

            // Observation Records DTOs
            CreateMap<ObservationRecord, ObservationRecordDto>();
            CreateMap<CreateObservationRecordDto, ObservationRecord>();
            CreateMap<UpdateObservationRecordDto, ObservationRecord>()
                .ForMember(dest => dest.AnimalId, opt => opt.Condition(src => src.AnimalId != null))
                .ForMember(dest => dest.AnimalId, opt => opt.MapFrom(src => src.AnimalId))
                .ForMember(dest => dest.Location, opt => opt.Condition(src => src.Location != null))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Timestamp, opt => opt.Condition(src => src.Timestamp != null))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.Comment, opt => opt.Condition(src => src.Comment != null))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment));

            // User DTOs
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>()
                .ForMember(u => u.PasswordHash, opt => opt.Ignore());

            CreateMap<Photo, PhotoDto>();

            CreateMap<Animal, AnimalDto>();
                // .ForMember(dest => dest.ObservationRecords, opt => opt.MapFrom(src => src.ObservationRecords));

            // Mapping for Photo and Comment (additional mappings for nested DTOs)
            CreateMap<PhotoDto, Photo>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));  // Assuming Photo has Url and Description

            // Mapping AnimalDto to Animal with ObservationRecords excluded (assuming it's not part of incoming data)
            CreateMap<AnimalDto, Animal>();
                // .ForMember(dest => dest.ObservationRecords, opt => opt.Ignore());  // ObservationRecords are excluded here

        }
    }
}