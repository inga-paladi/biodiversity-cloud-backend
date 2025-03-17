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

            CreateMap<ObservationDto, Observation>();
            CreateMap<PhotoDto, Photo>();
            CreateMap<CommentDto, Comment>();
        }
    }
}
