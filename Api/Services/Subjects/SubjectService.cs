using Api.Dto.Shared;
using Api.Services.Constant;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Subjects
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepo;
        private readonly IClassRepository _classRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(ILogger<SubjectService> logger, IClassRepository classRepo)
        {
            _subjectRepo = SubjectRepository.Instance;
            _classRepo = classRepo;
            _studentRepo = StudentRepository.Instance;
            _logger = logger;
        }

        public Task<ResponseDto<object>> CreateSubject(SubjectDto subjectDto)
        {
            _logger.LogInformation("createSubject(subjectDto: {})", subjectDto);

            if (subjectDto == null)
            {
                _logger.LogWarning("{}{}", "Create subject: ", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (_subjectRepo.ExistsByName(subjectDto.Name).Result)
            {
                _logger.LogWarning("{}{}", "Create subject: ", "Subject already exist.");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Subject already exist." });

            }
            Subject subject = new Subject
            {
                Id = subjectDto.Id,
                Name = subjectDto.Name,
                IsDisable = 0
            };
            _subjectRepo.Save(subject);
            _logger.LogInformation("Create subject success");
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
        }

        public Task<ResponseDto<object>> DeleteSubject(int id)
        {
            _logger.LogInformation("deleteSubject(id: {})", id);

            if (!_subjectRepo.ExistsById(id).Result)
            {
                _logger.LogWarning("{}{}", "Delete subject: ", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });

            }
            if (_classRepo.FindClassBySubjectAsync(id).Result != 0)
            {
                _logger.LogWarning("{}{}", "Delete subject: ", "Some classes use this subject");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Some classes use this subject" });
            }
            var rmSubject = _subjectRepo.FindById(id).Result;
            _subjectRepo.Delete(rmSubject);
            _logger.LogInformation("Delete subject success");
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }

        public Task<ResponseDto<HashSet<SubjectDto>>> GetSubjects()
        {
            _logger.LogInformation("getSubjects()");

            var subjects = _subjectRepo.FindAll().Result;
            var subjectDtos = subjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToHashSet();

            var res = new ResponseDto<HashSet<SubjectDto>>
            {
                code = ServiceStatusCode.OK_STATUS,
                message = ServiceMessage.SUCCESS_MESSAGE,
                data = subjectDtos
            };

            return Task.FromResult(res);
        }

        public Task<int> IsStudentStudySubject(string userEmail, string subjectName)
        {
            _logger.LogInformation("isStudentStudiedSubject(subjectName: {}, userEmail: {})", subjectName, userEmail);
            int studentId = _studentRepo.FindStudentIdByEmail(userEmail).Result;
            if (studentId == null || subjectName == null)
            {
                _logger.LogWarning("isStudentStudiedSubject: {}", ServiceStatusCode.BAD_REQUEST_STATUS);
                return Task.FromResult(500);
            }
            if (!_subjectRepo.ExistsByName(subjectName).Result)
            {
                _logger.LogWarning("isStudentStudiedSubject: {}", "Subject name not exist.");
                return Task.FromResult(500);
            }
            List<Class> classList = _subjectRepo.FindByName(subjectName).Result.Classes.ToList();
            foreach (Class classEntity in classList)
            {
                if (_classRepo.ExistInClassAsync(studentId, classEntity.Id) != null)
                {
                    _logger.LogInformation("isStudentStudiedSubject: {}", "Student study this subject.");
                    return Task.FromResult(200);
                }
            }
            _logger.LogInformation("isStudentStudiedSubject: {}", "Student not study this subject.");
            return Task.FromResult(202);
        }

        public Task<ResponseDto<object>> UpdateSubject(SubjectDto subjectDto)
        {
            _logger.LogInformation("updateSubject(subjectDto: {})", subjectDto);

            if (subjectDto == null)
            {
                _logger.LogWarning("{}{}", "Update subject: ", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (!_subjectRepo.ExistsById(subjectDto.Id).Result)
            {
                _logger.LogWarning("{}{}", "Update subject: ", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_subjectRepo.ExistsByName(subjectDto.Name).Result)
            {
                _logger.LogWarning("{}{}", "Create subject: ", "Subject already exist.");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Subject already exist." });
            }

            var subject = _subjectRepo.FindById(subjectDto.Id).Result;
            subject.Name = subjectDto.Name;
            _subjectRepo.Update(subject);

            _logger.LogInformation("Update subject success");
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
        }
    }
}
