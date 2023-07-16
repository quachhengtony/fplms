using Api.Dto.Request;
using Api.Dto.Response;
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

namespace Api.Services.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IClassRepository _classRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentGroupRepository _studentGroupRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly ILogger<GroupService> _logger;

        private const string GROUP_DISABLE = "Group is disable";
        private const string REMOVE_STUDENT_FROM_GROUP_MESSAGE = "Remove student from group: ";

        public GroupService(ILogger<GroupService> logger)
        {
            _classRepo = ClassRepository.Instance;
            _studentRepo = StudentRepository.Instance;
            _studentGroupRepo = StudentGroupRepository.Instance;
            _groupRepo = GroupRepository.Instance;
            _projectRepo = ProjectRepository.Instance;
            _logger = logger;
        }

        public Task<ResponseDto<object>> AddStudentToGroup(int classId, int groupId, int studentId)
        {
            _logger.LogInformation("addStudentToGroup(classId: {}, groupId: {}, studentId: {})", classId, groupId, studentId);

            if (classId == null || groupId == null || studentId == null)
            {
                _logger.LogWarning("Add student to group: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (_classRepo.ExistInClassAsync(studentId, classId).Result == 0 ||
                    _groupRepo.IsGroupExistsInClassAsync(groupId, classId).Result == 0)
            {
                _logger.LogWarning("Add student to group: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_groupRepo.IsGroupDisableAsync(groupId).Result == 1)
            {
                _logger.LogWarning("Add student to group: {}", GROUP_DISABLE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE });
            }
            if (_groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result != 0)
            {
                _logger.LogWarning("Add student to group: {} with id {}", "Student already joined other group", _groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Student already joined other group" });

            }
            if (_groupRepo.GetGroupLimitNumberAsync(groupId).Result <= _studentGroupRepo.GetCurrentNumberOfMemberInGroup(groupId).Result)
            {
                _logger.LogWarning("Add student to group: {}", "Group is full");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Group is full" });
            }
            if (_groupRepo.IsEnrollTimeOverAsync(groupId, System.DateTime.Now).Result == 0)
            {
                _logger.LogWarning("Add student to group: {}", "Enroll time is over");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Enroll time is over" });
            }

            if (_studentGroupRepo.FindLeaderInGroup(groupId).Result == 0)
                _studentGroupRepo.AddStudentInGroup(studentId, groupId, classId, 1); // 1 is Boolean.TRUE
            else
                _studentGroupRepo.AddStudentInGroup(studentId, groupId, classId, 0); // 0 is Boolean.FALSE

            _logger.LogInformation("Add student to group: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
        }

        public Task<ResponseDto<object>> ChangeGroupLeader(int classId, int leaderId, int newLeaderId)
        {
            _logger.LogInformation("changeGroupLeader(classId: {}, leaderId: {}, newLeaderId: {})", classId, leaderId, newLeaderId);

            if (classId == null || leaderId == null || newLeaderId == null)
            {
                _logger.LogWarning("Change group leader: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            int groupId = _groupRepo.FindGroupByStudentIdAndClassIdAsync(leaderId, classId).Result;
            if (groupId == 0)
            {
                _logger.LogWarning("Change group leader: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (_groupRepo.IsGroupDisableAsync(groupId).Result == 1)
            {
                _logger.LogWarning("Change group leader: {}", GROUP_DISABLE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE });
            }
            if (_studentGroupRepo.IsStudentExistInGroup(groupId, newLeaderId).Result == 0 ||
                    leaderId != _studentGroupRepo.FindLeaderInGroup(groupId).Result)
            {
                _logger.LogWarning("Change group leader: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_groupRepo.IsEnrollTimeOverAsync(groupId, System.DateTime.Now).Result == 0)
            {
                _logger.LogWarning("Change group leader: {}", "Enroll time is over");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Enroll time is over" });

            }
            _studentGroupRepo.UpdateGroupLeader(groupId, newLeaderId, 1);   //add new leader
            _studentGroupRepo.UpdateGroupLeader(groupId, leaderId, 0);     //remove old leader
            _logger.LogInformation("Change group leader: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }

        public Task<ResponseDto<object>> ChooseProjectInGroup(int classId, int projectId, int studentId)
        {
            _logger.LogInformation("chooseProjectInGroup(classId: {}, projectId: {}, studentId: {})", classId, projectId, studentId);

            if (classId == null || projectId == null || studentId == null)
            {
                _logger.LogWarning("Choose project in group: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            int groupId = _groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result;
            if (groupId == 0)
            {
                _logger.LogWarning("Choose project in group: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (_groupRepo.IsGroupDisableAsync(groupId).Result == 1)
            {
                _logger.LogWarning("Choose project in group: {}", GROUP_DISABLE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE });
            }
            if (studentId != _studentGroupRepo.FindLeaderInGroup(groupId).Result)
            {
                _logger.LogWarning("Choose project in group: {}", "Not a leader");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Not a leader" });
            }
            if (_projectRepo.ExistsById(projectId).Result == 0)
            {
                _logger.LogWarning("Choose project in group: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_groupRepo.IsEnrollTimeOverAsync(groupId, System.DateTime.Now).Result == 0)
            {
                _logger.LogWarning("Choose project in group: {}", "Enroll time is over");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Enroll time is over" });
            }
            _groupRepo.UpdateProjectInGroupAsync(groupId, projectId);
            _logger.LogInformation("Choose project in group success.");
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }

        public Task<ResponseDto<object>> CreateGroupRequestByLecturer(CreateGroupRequestDto createGroupRequest, string lecturerEmail)
        {
            _logger.LogInformation("Create group : {}", createGroupRequest);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(createGroupRequest.ClassId).Result != lecturerEmail)
            {
                _logger.LogWarning("Create group : {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            if (createGroupRequest.GroupQuantity == null || createGroupRequest.MemberQuantity == null)
            {
                _logger.LogWarning("Create group : {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });

            }

            //create group with amount quantity
            var temp = _groupRepo.GetMaxGroupNumberAsync(createGroupRequest.ClassId);
            int startGroupNumber = temp.Result;
            startGroupNumber++;
            for (int index = startGroupNumber; index<startGroupNumber + createGroupRequest.GroupQuantity; index++) 
            {
                Group group = new Group()
                {
                    MemberQuantity = createGroupRequest.MemberQuantity,
                    EnrollTime = createGroupRequest.EnrollTime,
                    ClassId = createGroupRequest.ClassId,
                    Number = index,
                    IsDisable = 0
                };
                _groupRepo.Add(group);
            }
            _logger.LogInformation("Create group : {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
        }

        public Task<ResponseDto<object>> DeleteGroupByLecturer(int groupId, int classId, string lecturerEmail)
        {
            _logger.LogInformation("Delete group : {}, {}", groupId, classId);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Delete group : {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            if (_groupRepo.IsGroupExistsInClassAsync(groupId, classId).Result == 0)
            {
                _logger.LogWarning("Delete group : {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_studentGroupRepo.FindLeaderInGroup(groupId).Result != 0)
            {
                _logger.LogWarning("Delete group : {}", "Group is not empty");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Group is not empty" });
            }
            else
            {
                _studentGroupRepo.DeleteAllStudentInGroup(groupId);
                var group = _groupRepo.FindOneByIdAsync(groupId).Result;
                _groupRepo.Delete(group);
                _logger.LogInformation("Delete group success");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
            }
        }

        public Task<ResponseDto<object>> DisableGroupByLecturer(int groupId, int classId, string lecturerEmail)
        {
            _logger.LogInformation("Disable group : {}, {}", groupId, classId);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Disable group : {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            if (_groupRepo.IsGroupExistsInClassAsync(groupId, classId).Result == 0)
            {
                _logger.LogWarning("Disable group : {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_groupRepo.IsGroupDisableAsync(groupId).Result == 1)
            {
                _logger.LogWarning("Disable group : {}", "Group already disable");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Group already disable" });
            }
            else
            {
                _groupRepo.SetGroupDisableAsync(groupId);
                _logger.LogInformation("Disable group success");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
            }
        }

        public Task<ResponseDto<object>> EnableGroupByLecturer(int groupId, int classId, string lecturerEmail)
        {
            _logger.LogInformation("Enable group : {}, {}", groupId, classId);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Enable group : {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            if (_groupRepo.IsGroupExistsInClassAsync(groupId, classId).Result == 0)
            {
                _logger.LogWarning("Enable group : {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_groupRepo.IsGroupDisableAsync(groupId).Result == 0)
            {
                _logger.LogWarning("Enable group : {}", "Group already enable");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Group already enable" });
            }
            else
            {
                _groupRepo.SetGroupEnableAsync(groupId);
                _logger.LogInformation("Enable group success");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });
            }
        }

        public Task<ResponseDto<GroupDetailResponseDto>> GetGroupByClassId(int classId, int studentId)
        {
            _logger.LogInformation("getGroupByGroupIdAndClassId(classId: {}, studentId: {})", classId, studentId);

            if (classId == null || studentId == null || _classRepo.FindOneByIdAsync(classId).Result.Id == 0)
            {
                _logger.LogWarning("Get group in class: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<GroupDetailResponseDto> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            int groupId = _groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result;
            if (groupId == 0)
            {
                _logger.LogWarning("Get group in class: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<GroupDetailResponseDto> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            Group group = _groupRepo.FindOneByIdAsync(groupId).Result;
            GroupDetailResponseDto groupDetailResponse = new GroupDetailResponseDto
            {
                Id = group.Id,
                Number = group.Number ?? 0,
                MemberQuantity = group.MemberQuantity ?? 0,
                CurrentNumber = _studentGroupRepo.GetCurrentNumberOfMemberInGroup(group.Id).Result,
                EnrollTime = group.EnrollTime,
                LeaderId = _studentGroupRepo.FindLeaderInGroup(group.Id).Result,
                IsDisable = group.IsDisable != 0
            };
            groupDetailResponse.StudentDtoSet = group.StudentGroups.Select(sg => new StudentDto
            {
                Id = sg.StudentId,
                Name = sg.Student.Name,
                Code = sg.Student.Code,
                Email = sg.Student.Email
            }).ToHashSet();
            if (group.ProjectId != null)
            {
                groupDetailResponse.ProjectDTO = new ProjectDto
                {
                    Id = group.ProjectId ?? 0,
                    Name = group.Project.Name,
                    Theme = group.Project.Theme,
                    Actors = group.Project.Actors,
                    Requirements = group.Project.Requirements,
                    Context = group.Project.Context,
                    SemesterCode = group.Project.SemesterCode,
                    Problem = group.Project.Problem,
                    SubjectId = group.Project.SubjectId
                };
            }

            _logger.LogInformation("Get group in class: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<GroupDetailResponseDto> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = groupDetailResponse });

        }

        public Task<ResponseDto<HashSet<GroupDetailResponseDto>>> GetGroupOfClassByLecturer(int classId, string lecturerEmail)
        {
            _logger.LogInformation("Get group of class: {}", classId);
            //check if the class not of the lecturer  
            if (lecturerEmail != _classRepo.FindLecturerEmailOfClassAsync(classId).Result)
            {
                _logger.LogWarning("Get group of class: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE});

            }
            Class classEntity = _classRepo.FindOneByIdAsync(classId).Result;
            if (classEntity == null)
            {
                _logger.LogWarning("Get group of class: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            var groupDetailResponses = classEntity.Groups.Select(g => new GroupDetailResponseDto
            {
                Id = g.Id,
                Number = g.Number ?? 0,
                MemberQuantity = g.MemberQuantity ?? 0,
                CurrentNumber = _studentGroupRepo.GetCurrentNumberOfMemberInGroup(g.Id).Result,
                EnrollTime = g.EnrollTime,
                ProjectDTO = g.ProjectId == null ? null : new ProjectDto
                {
                    Id = g.Project.Id,
                    Name = g.Project.Name,
                    Theme = g.Project.Theme,
                    Problem = g.Project.Problem,
                    Context = g.Project.Context,
                    Actors = g.Project.Actors,
                    Requirements = g.Project.Requirements,
                    SubjectId = g.Project.SubjectId ?? 0,
                    SemesterCode = g.Project.SemesterCode,
                },
                LeaderId = _studentGroupRepo.FindLeaderInGroup(g.Id).Result,
                IsDisable = (g.IsDisable ?? 0) != 0
            }).ToHashSet();

            _logger.LogInformation("Get group of class: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = groupDetailResponses });

        }

        public Task<ResponseDto<HashSet<GroupDetailResponseDto>>> GetGroupOfClassByStudent(int classId, string studentEmail)
        {
            _logger.LogInformation("Get group of class: {}", classId);
            //check if the class not of the student
            int studentId = _studentRepo.FindStudentIdByEmail(studentEmail).Result;
            if (_classRepo.ExistInClassAsync(studentId, classId).Result == 0)
            {
                _logger.LogWarning("Get group of class: {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            Class classEntity = _classRepo.FindOneByIdAsync(classId).Result;
            if (classEntity == null)
            {
                _logger.LogWarning("Get group of class: {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            var groupDetailResponses = classEntity.Groups.Select(g => new GroupDetailResponseDto
            {
                Id = g.Id,
                Number = g.Number ?? 0,
                MemberQuantity = g.MemberQuantity ?? 0,
                CurrentNumber = _studentGroupRepo.GetCurrentNumberOfMemberInGroup(g.Id).Result,
                EnrollTime = g.EnrollTime,
                ProjectDTO = g.ProjectId == null ? null : new ProjectDto
                {
                    Id = g.Project.Id,
                    Name = g.Project.Name,
                    Theme = g.Project.Theme,
                    Problem = g.Project.Problem,
                    Context = g.Project.Context,
                    Actors = g.Project.Actors,
                    Requirements = g.Project.Requirements,
                    SubjectId = g.Project.SubjectId ?? 0,
                    SemesterCode = g.Project.SemesterCode,
                },
                LeaderId = _studentGroupRepo.FindLeaderInGroup(g.Id).Result,
                IsDisable = (g.IsDisable ?? 0) != 0
            }).ToHashSet();

            _logger.LogInformation("Get group of class: {}", ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<HashSet<GroupDetailResponseDto>> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE, data = groupDetailResponses });

        }

        public Task<ResponseDto<object>> RemoveStudentFromGroup(int classId, int studentId)
        {
            _logger.LogInformation("removeStudentFromGroup(classId: {}, studentId: {})", classId, studentId);

            if (classId == null || studentId == null)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (_classRepo.ExistInClassAsync(studentId, classId).Result == 0)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });

            }
            int groupId = _groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result;
            if (groupId == 0)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, "Student not in group");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Student not in group" });

            }
            if (_groupRepo.IsGroupDisableAsync(groupId).Result == 1)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, GROUP_DISABLE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE });

            }
            if (_groupRepo.IsEnrollTimeOverAsync(groupId, System.DateTime.Now).Result == 0)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, "Enroll time is over");
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Enroll time is over" });
            }
            
            if (studentId == _studentGroupRepo.FindLeaderInGroup(groupId).Result)
            {
                _studentGroupRepo.DeleteStudentInGroup(studentId, classId);
                int newLeaderId = _studentGroupRepo.ChooseRandomGroupMember(groupId).Result;
                if (newLeaderId != 0)
                {
                    _studentGroupRepo.AddRandomGroupLeader(groupId, newLeaderId);
                }
            } else
            {
                _studentGroupRepo.DeleteStudentInGroup(studentId, classId);
            }

            _logger.LogInformation("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, ServiceMessage.SUCCESS_MESSAGE);
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }

        public Task<ResponseDto<object>> RemoveStudentFromGroupByLeader(int classId, int studentId, int leaderId)
        {
            _logger.LogInformation("removeStudentFromGroupByLeader(classId: {}, studentId: {}, leaderId: {})", classId, studentId, leaderId);

            if (classId == null || studentId == null || leaderId == null)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            int groupId = _groupRepo.FindGroupByStudentIdAndClassIdAsync(studentId, classId).Result;
            if (groupId == 0)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }
            if (leaderId != _studentGroupRepo.FindLeaderInGroup(groupId).Result)
            {
                _logger.LogWarning("{}{}", REMOVE_STUDENT_FROM_GROUP_MESSAGE, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = ServiceMessage.INVALID_ARGUMENT_MESSAGE });
            }

            return RemoveStudentFromGroup(classId, studentId);
        }

        public Task<ResponseDto<object>> UpdateGroupByLecturer(int classId, GroupDto groupDTO, string lecturerEmail)
        {
            _logger.LogInformation("Update group : {}", groupDTO);
            //check if the class not of the lecturer
            if (_classRepo.FindLecturerEmailOfClassAsync(classId).Result != lecturerEmail)
            {
                _logger.LogWarning("Update group : {}", ServiceMessage.FORBIDDEN_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.FORBIDDEN_STATUS, message = ServiceMessage.FORBIDDEN_MESSAGE });
            }
            int groupNumber = _groupRepo.FindGroupNumberAsync(groupDTO.Id, classId).Result;
            if (groupDTO.Id == 0 || groupNumber == 0)
            {
                _logger.LogWarning("Update group : {}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.NOT_FOUND_STATUS, message = ServiceMessage.ID_NOT_EXIST_MESSAGE });
            }
            if (_groupRepo.IsGroupDisableAsync(groupDTO.Id).Result == 1)
            {
                _logger.LogWarning("Update group : {}", GROUP_DISABLE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = GROUP_DISABLE });
            }
            // not accept chane in the same group
            if (groupNumber == groupDTO.Number)
            {
                _logger.LogWarning("Update group: {}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.BAD_REQUEST_STATUS, message = "Group number already exist" });
            }

            Group group = _groupRepo.FindOneByIdAsync(groupDTO.Id).Result;
            group.Number = groupDTO.Number;
            group.MemberQuantity = groupDTO.MemberQuantity;
            group.EnrollTime = groupDTO.EnrollTime;
            if (groupDTO.ProjectDto != null)
            {
                group.ProjectId = groupDTO.ProjectDto.Id;
            }
            _groupRepo.Update(group);
            
            _logger.LogInformation("Update group success");
            return Task.FromResult(new ResponseDto<object> { code = ServiceStatusCode.OK_STATUS, message = ServiceMessage.SUCCESS_MESSAGE });

        }
    }
}
