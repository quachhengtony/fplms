using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dto.Response;
using Api.Dto.Shared;
using Api.Services.Constant;
using Api.Services.Groups;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using FPLMS.Api.Services;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;

namespace Api.Services.Classes
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentGroupRepository _studentGroupRepo;
        private readonly ISubjectRepository _subjectRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly ILecturerRepository _lecturerRepo;
        private readonly IGroupService _groupService;
        private readonly ISemesterRepository _semesterRepo;
        private readonly ILogger<ClassService> _logger;

        public ClassService(IGroupService groupService, ILogger<ClassService> logger)
        {
            _classRepo = ClassRepository.Instance;
            _studentGroupRepo = StudentGroupRepository.Instance;
            _studentRepo = StudentRepository.Instance;
            _subjectRepo = SubjectRepository.Instance;
            _groupRepo = GroupRepository.Instance;
            _lecturerRepo = LecturerRepository.Instance;
            _semesterRepo = SemesterRepository.Instance;
            _groupService = groupService;
            _logger = logger;

        }

        public Task<ResponseDto<object>> ChangeStudentGroupByLecturer(int classId, int studentId, int groupNumber, string lecturerEmail)
        {
            _logger.LogInformation("Change student group: {}, {}", classId, studentId);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Change student group: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            if (_classRepo.ExistInClassAsync(studentId, classId).Result == 0) //exist
            {
                _logger.LogWarning("Change student group: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            _studentGroupRepo.UpdateStudentGroup(studentId, classId, groupNumber);
            _logger.LogInformation("Change student group: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
        }

        public Task<ResponseDto<int>> CreateClassByLecturer(ClassDto classDto, string lecturerEmail)
        {
            _logger.LogInformation("Create class: {}", classDto);
            if (!_subjectRepo.ExistsById(classDto.SubjectId).Result)
            {
                _logger.LogWarning("Create class: {}", "Subject not exist.");
                return Task.FromResult(new ResponseDto<int> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (!_semesterRepo.ExistsById(classDto.SemesterCode).Result)
            {
                _logger.LogWarning("Create class: {}", "Semester not exist.");
                return Task.FromResult(new ResponseDto<int> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Semester not exist." });
            }
            if (_semesterRepo.GetSemesterEndDate(classDto.SemesterCode).Result < DateTime.Now)
            {
                _logger.LogWarning("Create class: {}", "Semester already end.");
                return Task.FromResult(new ResponseDto<int> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Semester already end." });
            }

            Class _class = new Class
            {
                Name = classDto.Name,
                CycleDuration = classDto.CycleDuration,
                SemesterCode = classDto.SemesterCode,
                EnrollKey = classDto.EnrollKey,
                SubjectId = classDto.SubjectId,
                IsDisable = 0,
                LecturerId = _lecturerRepo.FindLecturerIdByEmailAsync(lecturerEmail).Result
            };

            _classRepo.Add(_class);
            
            _logger.LogInformation("Create class success");
            return Task.FromResult(new ResponseDto<int> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = _class.Id });
        }

        public Task<ResponseDto<object>> DeleteClassByLecturer(int classId, string lecturerEmail)
        {
            _logger.LogInformation("Delete class: {}", classId);
            //check if the class not of the lecturer
            if (_classRepo.FindOneByIdAsync(classId).Result == null)
            {
                _logger.LogWarning("Delete class: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Delete class: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }

            Class classEntity = _classRepo.FindOneByIdAsync(classId).Result;
            classEntity.IsDisable = 1; //delete class
            _classRepo.Update(classEntity);

            _logger.LogInformation("Delete class success");
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }

        public Task<ResponseDto<object>> EnrollStudentToClass(int classId, int studentId, string enrollKey)
        {
            _logger.LogInformation("enrollStudentToClass(classId: {}, studentId: {}, enrollKey: {})", classId, studentId, enrollKey);

            if (_classRepo.FindOneByIdAsync(classId).Result == null || _studentRepo.FindOneById(studentId).Result == null)
            {
                _logger.LogWarning("Enroll student to class: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            /*if (_classRepo.FindOneByIdAsync(classId).Result.IsDisable == 1)
            {
                _logger.LogWarning("Enroll student to class: {}", "Class is disable");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Class is disable" });
            }*/
            DateTime startDate = _semesterRepo.GetSemesterStartDate(_classRepo.GetClassSemesterAsync(classId).Result).Result;
            DateTime endDate = _semesterRepo.GetSemesterEndDate(_classRepo.GetClassSemesterAsync(classId).Result).Result;
            if (DateTime.Now < startDate)
            {
                _logger.LogWarning("Enroll student to class: {}", "Class not open yet");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Class not open yet" });

            }
            if (DateTime.Now > endDate)
            {
                _logger.LogWarning("Enroll student to class: {}", "Enroll time is over");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Enroll time is over" });
            }
            if (_classRepo.GetClassEnrollKeyAsync(classId).Result != enrollKey)
            {
                _logger.LogWarning("Enroll student to class: {}", "Enroll key is not correct");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Enroll key is not correct" });
            }
            if (_classRepo.ExistInClassAsync(studentId, classId).Result != 0)
            {
                _logger.LogWarning("Enroll student to class: {}", "Student already joined this class.");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Student already joined this class." });
            }
            _classRepo.InsertStudentInClassAsync(studentId, classId);
            _logger.LogInformation("Enroll student to class: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }

        public Task<ResponseDto<ClassDto>> GetClassDetailByLecture(string lecturerEmail, int classId)
        {
            _logger.LogInformation("getClassDetailByLecture(classId: {}, lecturerEmail: {})", classId, lecturerEmail);

            if (_classRepo.FindOneByIdAsync(classId).Result == null)
            {
                _logger.LogWarning("Get class detail by lecturer: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<ClassDto> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Get class detail by lecturer: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<ClassDto> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            Class classEntity = _classRepo.FindOneByIdAsync(classId).Result;

            ClassDto classDto = new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name ?? "",
                CycleDuration = classEntity.CycleDuration ?? 0,
                SemesterCode = classEntity.SemesterCode,
                EnrollKey = classEntity.EnrollKey ?? "",
                SubjectId = classEntity.SubjectId
            };

            _logger.LogInformation("Get class detail by lecturer: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<ClassDto> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = classDto });
        }

        public Task<ResponseDto<ClassDto>> GetClassDetailByStudent(string studentEmail, int classId)
        {
            _logger.LogInformation("getClassDetailByLecture(classId: {}, studentEmail: {})", classId, studentEmail);
            int studentId = _studentRepo.FindStudentIdByEmail(studentEmail).Result;
            if (studentId == 0)
            {
                _logger.LogWarning("Get class detail by student: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<ClassDto> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (_classRepo.FindOneByIdAsync(classId).Result == null)
            {
                _logger.LogWarning("Get class detail by student: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<ClassDto> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });

            }
            if (_classRepo.ExistInClassAsync(studentId, classId).Result == 0)
            {
                _logger.LogWarning("Get class detail by student: {}", "Student is not in this class");
                return Task.FromResult(new ResponseDto<ClassDto> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = "Student is not in this class" });

            }
            Class classEntity = _classRepo.FindOneByIdAsync(classId).Result;

            ClassDto classDto = new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name ?? "",
                CycleDuration = classEntity.CycleDuration ?? 0,
                SemesterCode = classEntity.SemesterCode,
                EnrollKey = classEntity.EnrollKey ?? "",
                SubjectId = classEntity.SubjectId
            };

            _logger.LogInformation("Get class detail by student: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<ClassDto> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = classDto });
        }

        public Task<ResponseDto<HashSet<ClassByStudentResponseDto>>> GetClassesBySearchStrByStudent(string? search, int studentId)
        {
            _logger.LogInformation("getClassesBySearchStrByStudent(studentId: {}, search: {})", studentId, search);

            if (studentId == null)
            {
                _logger.LogWarning("Get class by student: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<ClassByStudentResponseDto>> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (search == null) search = "";
            var classSet = _classRepo.GetClassBySearchStrAsync(search).Result;
            var classByStudentResponseSet = classSet.Select(c =>
            {
                var temp = new ClassByStudentResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    SemesterCode = c.SemesterCode,
                    HasEnrollKey = c.EnrollKey == null ? false : true,
                    SubjectId = c.SubjectId,
                    LecturerDto = new LecturerDto
                    {
                        Email = c.Lecturer.Email,
                        Name = c.Lecturer.Name
                    }
                };
                temp.IsJoin = _classRepo.ExistInClassAsync(studentId, c.Id).Result != 0;
                return temp;
            }).ToHashSet();

            _logger.LogInformation("Get class by student: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<HashSet<ClassByStudentResponseDto>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = classByStudentResponseSet });

        }

        public Task<ResponseDto<HashSet<ClassDto>>> GetClassOfLecture(string lectureEmail)
        {
            Lecturer lecturer = _lecturerRepo.FindOneByEmailAsync(lectureEmail).Result;
            if (lecturer == null)
            {
                _logger.LogWarning("Get class of lecturer: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<ClassDto>> { code = ServiceStatusCode.UNAUTHENTICATED_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            var classDtoSet = lecturer.Classes.Where(c => c.IsDisable == 0).Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name,
                CycleDuration = c.CycleDuration ?? 0,
                SemesterCode = c.SemesterCode,
                EnrollKey = c.EnrollKey,
                SubjectId = c.SubjectId
            }).ToHashSet();
            classDtoSet = classDtoSet.Where(c => c != null).ToHashSet();

            _logger.LogInformation("Get class of lecturer: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<HashSet<ClassDto>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = classDtoSet });
        }

        public Task<ResponseDto<HashSet<StudentInClassResponseDto>>> GetStudentInClassByLecturer(int classId, string lecturerEmail)
        {
            if (classId == null)
            {
                _logger.LogWarning("Get student in class: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<StudentInClassResponseDto>> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            _logger.LogInformation("Get student in class: {}", classId);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Get student in class: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<StudentInClassResponseDto>> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }

            var studentSet = _classRepo.FindOneByIdAsync(classId).Result.Students;
            HashSet<StudentInClassResponseDto> studentInClassResponseSet;
            if (studentSet == null)
            {
                _logger.LogWarning("Get student in class: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<StudentInClassResponseDto>> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            } else
            {
                studentInClassResponseSet = studentSet.Select(s =>
                {
                    var group = _studentRepo.FindGroupByStudentIdAndClassId(s.Id, classId).Result;
                    if (group == null)
                    {
                        return new StudentInClassResponseDto
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Email = s.Email,
                            Code = s.Code,
                            GroupId = 0,
                            GroupNumber = 0,
                            IsLeader = false
                        };
                    }
                    return new StudentInClassResponseDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Email = s.Email,
                        Code = s.Code,
                        GroupId = group.Id,
                        GroupNumber = group.Number ?? 0,
                        IsLeader = _studentGroupRepo.FindStudentLeaderRoleInClass(s.Id, classId).Result == 1
                    };
                }).ToHashSet();
            }

            _logger.LogInformation("Get student in class: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<HashSet<StudentInClassResponseDto>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = studentInClassResponseSet });
        }

        public Task<ResponseDto<object>> RemoveStudentInClassByLecturer(int studentId, int classId, string lecturerEmail)
        {
            if (studentId == null)
            {
                _logger.LogWarning("Remove student in class: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            _logger.LogInformation("Remove student in class: {}, {}", classId, studentId);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Remove student in class: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            if (_classRepo.ExistInClassAsync(studentId, classId).Result != 0) //exist
            {
                int groupId = _groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result;
                if (groupId != 0)
                {
                    _groupService.RemoveStudentFromGroup(classId, studentId);
                }
                _classRepo.DeleteStudentInClassAsync(studentId, classId);
                _logger.LogInformation("Remove student in class: {}", ServiceMessage.SUCCESS_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
            }
            else
            {
                _logger.LogWarning("Remove student in class: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
        }

        public Task<ResponseDto<object>> UnenrollStudentInClass(int studentId, int classId)
        {
            _logger.LogInformation("removeStudentInClass(studentId: {}, classId: {})", studentId, classId);
            if (studentId == 0)
            {
                _logger.LogWarning("Remove student in class: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (_classRepo.ExistInClassAsync(studentId, classId).Result != 0) //exist
            {
                int groupId = _groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result;
                if (groupId != 0)
                {
                    _groupService.RemoveStudentFromGroup(classId, studentId);
                }
                    
                _classRepo.DeleteStudentInClassAsync(studentId, classId);
                _logger.LogInformation("Remove student in class: {}", ServiceMessage.SUCCESS_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
            }
            else
            {
                _logger.LogWarning("Remove student in class: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });

            }
        }

        public Task<ResponseDto<object>> UpdateClassByLecturer(ClassDto classDto, string lecturerEmail)
        {
            _logger.LogInformation("Update class: {}", classDto);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classDto.Id).Result != lecturerEmail)
            {
                _logger.LogWarning("Update class: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            if (_classRepo.FindOneByIdAsync(classDto.Id).Result.Id == 0)
            {
                _logger.LogWarning("Update class: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (!_semesterRepo.ExistsById(classDto.SemesterCode).Result)
            {
                _logger.LogWarning("Update class: {}", "Semester not exist.");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Semester not exist." });

            }
            if (!_subjectRepo.ExistsById(classDto.SubjectId).Result)
            {
                _logger.LogWarning("Update class: {}", "Subject not exist.");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Subject not exist." });

            }

            Class _class = _classRepo.FindOneByIdAsync(classDto.Id).Result;
            _class.Name = classDto.Name;
            _class.CycleDuration = classDto.CycleDuration;
            _class.SemesterCode = classDto.SemesterCode;
            _class.EnrollKey = classDto.EnrollKey;
            _class.SubjectId = classDto.SubjectId;
            _class.IsDisable = 0;
            _class.LecturerId = _lecturerRepo.FindLecturerIdByEmailAsync(lecturerEmail).Result;

            _classRepo.Update(_class);
            _logger.LogInformation("Update class success");
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }
    }
}
