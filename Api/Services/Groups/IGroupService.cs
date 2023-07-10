using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dto.Request;
using Api.Dto.Response;
using Api.Dto.Shared;
using FPLMS.Api.Dto;

namespace Api.Services.Groups
{
    public interface IGroupService
    {
        Task<ResponseDto<object>> CreateGroupRequestByLecturer(CreateGroupRequestDto createGroupRequest, string lecturerEmail);
        Task<ResponseDto<object>> UpdateGroupByLecturer(int classId, GroupDto groupDTO, string lecturerEmail);
        Task<ResponseDto<object>> DeleteGroupByLecturer(int groupId, int classId, string lecturerEmail);
        Task<ResponseDto<object>> DisableGroupByLecturer(int groupId, int classId, string lecturerEmail);
        Task<ResponseDto<object>> EnableGroupByLecturer(int groupId, int classId, string lecturerEmail);
        Task<ResponseDto<HashSet<GroupDetailResponseDto>>> GetGroupOfClassByLecturer(int classId, string lecturerEmail);
        Task<ResponseDto<HashSet<GroupDetailResponseDto>>> GetGroupOfClassByStudent(int classId, string studentEmail);
        Task<ResponseDto<object>> AddStudentToGroup(int classId, int groupId, int studentId);
        Task<ResponseDto<object>> RemoveStudentFromGroupByLeader(int classId, int studentId, int leaderId);
        Task<ResponseDto<object>> RemoveStudentFromGroup(int classId, int studentId);
        Task<ResponseDto<GroupDetailResponseDto>> GetGroupByClassId(int classId, int studentId);
        Task<ResponseDto<object>> ChangeGroupLeader(int classId, int leaderId, int newLeaderId);
        Task<ResponseDto<object>> ChooseProjectInGroup(int classId, int projectId, int studentId);

    }
}
