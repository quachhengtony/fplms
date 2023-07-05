using Api.Dto.Shared.plms.ManagementService.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Semesters
{
    public interface ISemesterService
    {
        Task AddSemester(SemesterDTO semesterDto);
        Task<HashSet<SemesterDTO>> GetSemester(string code);
        Task UpdateSemester(SemesterDTO semesterDto);
        Task DeleteSemester(string code);
    }
}
