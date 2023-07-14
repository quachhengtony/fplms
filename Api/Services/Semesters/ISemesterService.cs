using Api.Dto.Shared.plms.ManagementService.Model.DTO;
using FPLMS.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Semesters
{
    public interface ISemesterService
    {
        Task<ResponseDto<object>> AddSemester(SemesterDTO semesterDto);
        Task<ResponseDto<List<SemesterDTO>>> GetSemester(string code);
        Task<ResponseDto<object>> UpdateSemester(SemesterDTO semesterDto);
        Task<ResponseDto<object>> DeleteSemester(string code);
    }
}
