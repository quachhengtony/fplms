using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dto.Response;
using Api.Dto.Shared;
using FPLMS.Api.Dto;

namespace Api.Services.Classes
{
    public interface IClassService
    {
        Task<ResponseDto<int>> CreateClassByLecturer(ClassDto ClassDto, string lecturerEmail);
        Task<ResponseDto<object>> UpdateClassByLecturer(ClassDto ClassDto, string lecturerEmail);
        Task<ResponseDto<object>> DeleteClassByLecturer(int classId, string lecturerEmail);
        Task<ResponseDto<HashSet<ClassDto>>> GetClassOfLecture(string lectureEmail);
        Task<ResponseDto<ClassDto>> GetClassDetailByLecture(string lecturerEmail, int classId);
        Task<ResponseDto<ClassDto>> GetClassDetailByStudent(string studentEmail, int classId);
        Task<ResponseDto<HashSet<StudentInClassResponseDto>>> GetStudentInClassByLecturer(int classId, string lecturerEmail);
        Task<ResponseDto<object>> RemoveStudentInClassByLecturer(int studentId, int classId, string lecturerEmail);
        Task<ResponseDto<object>> ChangeStudentGroupByLecturer(int classId, int studentId, int groupNumber, string lecturerEmail);
        Task<ResponseDto<object>> EnrollStudentToClass(int classId, int studentId, string enrollKey);
        Task<ResponseDto<HashSet<ClassByStudentResponseDto>>> GetClassesBySearchStrByStudent(string search, int studentId);
        Task<ResponseDto<object>> UnenrollStudentInClass(int studentId, int classId);
    }
}

