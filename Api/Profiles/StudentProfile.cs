using AutoMapper;
using Api.Dto.Temp;
using BusinessObjects.Models;

namespace DiscussionService.Profiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<CreateStudentDto, Student>();
            CreateMap<Student, GetStudentDto>();
        }
    }
}