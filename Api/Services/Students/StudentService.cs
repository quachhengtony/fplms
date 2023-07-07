using Api.Dto.Shared;
using Api.Services.Constant;
using FPLMS.Api.Dto;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Api.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ILogger<StudentService> _logger;

        public StudentService(ILogger<StudentService> logger)
        {
            _studentRepo = StudentRepository.Instance;
            _logger = logger;
        }


        public Task<ResponseDto<StudentDto>> GetStudentById(int studentId)
        {
            _logger.LogInformation("getStudentById(studentId: {})", studentId);

            var student = _studentRepo.FindOneById(studentId).Result;
            if (student == null)
            {
                _logger.LogWarning("Get student by id: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<StudentDto> { code = ServiceStatusCode.UNAUTHENTICATED_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }

            StudentDto studentDto = new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Code = student.Code
            };

            _logger.LogInformation("Get student by id: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<StudentDto> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = studentDto });
        }

        public Task<int> GetStudentIdByEmail(string email)
        {
            _logger.LogInformation("getStudentIdByEmail(email: {})", email);
            return _studentRepo.FindStudentIdByEmail(email);
        }
    }
}
