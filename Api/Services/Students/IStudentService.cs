using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dto.Shared;
using FPLMS.Api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Students
{
    public interface IStudentService
    {
        Task<ResponseDto<StudentDto>> GetStudentById(int studentId);
        Task<int> GetStudentIdByEmail(string email);
    }
}
