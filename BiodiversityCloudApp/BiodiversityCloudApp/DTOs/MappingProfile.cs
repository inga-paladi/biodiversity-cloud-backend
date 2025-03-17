using System.Runtime.CompilerServices;
using AutoMapper;

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

            // DTO to Entity mapping (for incoming data)
            CreateMap<UserDto, User>()
                .ForMember(u => u.PasswordHash, opt => opt.Ignore());

            CreateMap<Observation, ObservationDto>()
                .ForMember(dest => dest.DateObserved, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.ObserverName, opt => opt.MapFrom(src => src.User.Name)) // ✅ Map from User
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

            CreateMap<ObservationDto, Observation>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.DateObserved));
            CreateMap<PhotoDto, Photo>();
            CreateMap<CommentDto, Comment>();
        }
    }
}
