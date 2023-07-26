using AutoMapper;
using Api.Dto.Temp;
using BusinessObjects.Models;

namespace DiscussionService.Profiles
{
    public class LecturerProfile : Profile
    {
        public LecturerProfile()
        {
            CreateMap<CreateLecturerDto, Lecturer>();
            CreateMap<Lecturer, GetLecturerDto>();
        }
    }
}