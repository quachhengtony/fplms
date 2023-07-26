using AutoMapper;
using Api.Dto.Temp;
using BusinessObjects.Models;
namespace DiscussionService.Profiles
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<CreateSubjectDto, Subject>();
            CreateMap<Subject, GetSubjectDto>();
        }
    }
}