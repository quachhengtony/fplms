using System.Collections.Generic;
using System.Threading.Tasks;
using FPLMS.Api.Dto;
using FPLMS.Api.Services;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;

public class ClassService : IClassService
{
    /*
    private readonly IClassRepository _classRepo;
    private readonly IStudentRepository _studentRepo;
    private readonly IStudentGroupRepository _studentGroupRepo;
    private readonly ISubjectRepository _subjectRepo;
    private readonly IGroupRepository _groupRepo;
    private readonly ILecturerRepository _lecturerRepo;
    //private readonly IClassRepository _classRepo;
    private readonly ISemesterRepository _semesterRepo;
    private readonly ILogger<ClassService> _logger;

    public ClassService(IClassRepository classRepo,
                            IStudentRepository studentRepo,
                            IStudentGroupRepository studentGroupRepo,
                            ISubjectRepository subjectRepo,
                            IGroupRepository groupRepo,
                            ILecturerRepository lecturerRepo,
                            ISemesterRepository semesterRepo,
                            ILogger<ClassService> logger)
    {
        _classRepo = classRepo;
        _studentGroupRepo = studentGroupRepo;
        _studentRepo = studentRepo;
        _subjectRepo = subjectRepo;
        _groupRepo = groupRepo;
        _lecturerRepo = lecturerRepo;
        _semesterRepo = semesterRepo;
        _logger = logger;

    }*/


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