using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Semester
{
    public interface ISemesterService
    {
        Response<Void> AddSemester(SemesterDTO semesterDto);
        Response<Set<SemesterDTO>> GetSemester(string code);
        Response<Void> UpdateSemester(SemesterDTO semesterDto);
        Response<Void> DeleteSemester(string code);
    }
}
