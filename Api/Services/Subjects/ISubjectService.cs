using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dto.Shared;
using FPLMS.Api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Subjects
{
    public interface ISubjectService
    {
        Task<ResponseDto<HashSet<SubjectDto>>> GetSubjects();
        Task<ResponseDto<object>> CreateSubject(SubjectDto subjectDto);
        Task<ResponseDto<object>> UpdateSubject(SubjectDto subjectDto);
        Task<ResponseDto<object>> DeleteSubject(int id);
        Task<int> IsStudentStudySubject(string userEmail, string subjectName);
    }
}
