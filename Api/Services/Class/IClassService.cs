using System.Collections.Generic;
using System.Threading.Tasks;
namespace FPLMS.Api.Services;

using FPLMS.Api.Dto;

public interface IClassService
{
    Task<ResponseDto<int>> CreateClassByLecturer(ClassRequestResponseDto ClassDto, string lecturerEmail);
    Task UpdateClassByLecturer(ClassRequestResponseDto ClassDto, string lecturerEmail);
    Task DeleteClassByLecturer(int classId, string lecturerEmail);
    Task<ResponseDto<HashSet<ClassRequestResponseDto>>> GetClassOfLecture(string lectureEmail);
    Task<ResponseDto<ClassRequestResponseDto>> GetClassDetailByLecture(string lecturerEmail, int classId);
    Task<ResponseDto<ClassRequestResponseDto>> GetClassDetailByStudent(string studentEmail, int classId);
    Task<ResponseDto<HashSet<StudentInClassResponseDto>>> GetStudentInClassByLecturer(int classId, string lecturerEmail);
    Task RemoveStudentInClassByLecturer(int studentId, int classId, string lecturerEmail);
    Task ChangeStudentGroupByLecturer(int classId, int studentId, int groupNumber, string lecturerEmail);
    Task EnrollStudentToClass(int classId, int studentId, string enrollKey);
}