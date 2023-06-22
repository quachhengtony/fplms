using System.Collections.Generic;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Services;

public class ClassService : IClassService
{
    public Task ChangeStudentGroupByLecturer(int classId, int studentId, int groupNumber, string lecturerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task<ResponseDto<int>> CreateClassByLecturer(ClassRequestResponseDto ClassDto, string lecturerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteClassByLecturer(int classId, string lecturerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task EnrollStudentToClass(int classId, int studentId, string enrollKey)
    {
        throw new System.NotImplementedException();
    }

    public Task<ResponseDto<ClassRequestResponseDto>> GetClassDetailByLecture(string lecturerEmail, int classId)
    {
        throw new System.NotImplementedException();
    }

    public Task<ResponseDto<ClassRequestResponseDto>> GetClassDetailByStudent(string studentEmail, int classId)
    {
        throw new System.NotImplementedException();
    }

    public Task<ResponseDto<HashSet<ClassRequestResponseDto>>> GetClassOfLecture(string lectureEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task<ResponseDto<HashSet<StudentInClassResponseDto>>> GetStudentInClassByLecturer(int classId, string lecturerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task RemoveStudentInClassByLecturer(int studentId, int classId, string lecturerEmail)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateClassByLecturer(ClassRequestResponseDto ClassDto, string lecturerEmail)
    {
        throw new System.NotImplementedException();
    }
}