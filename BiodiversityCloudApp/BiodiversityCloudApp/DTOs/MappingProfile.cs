using System.Runtime.CompilerServices;
using AutoMapper;
using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            //Entity to DTO mapping
            CreateMap<User, UserDto>();
            CreateMap<Observation, ObservationDto>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Animal, AnimalDto>();
            CreateMap<MicroObservation, MicroObservationDto>();

            CreateMap<MicroObservation, MicroObservationDto>()
                .ForMember(dest => dest.Animal, opt => opt.MapFrom(src => src.Animal)); // optional

            CreateMap<MicroObservationDto, MicroObservation>()
                .ForMember(dest => dest.Animal, opt => opt.Ignore()); // EF will link it by AnimalId

            CreateMap<Animal, AnimalDto>().ReverseMap();

            // DTO to Entity mapping (for incoming data)
            CreateMap<UserDto, User>()
                .ForMember(u => u.PasswordHash, opt => opt.Ignore());

            CreateMap<Observation, ObservationDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ObserverName, opt => opt.MapFrom(src => src.User.Name)) 
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<ObservationDto, Observation>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
            CreateMap<PhotoDto, Photo>();
            CreateMap<CommentDto, Comment>();
        }
    }
}
