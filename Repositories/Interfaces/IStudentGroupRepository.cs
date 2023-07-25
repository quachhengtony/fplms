using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IStudentGroupRepository
    {
        Task DeleteStudentInGroup(int studentId, int classId);
        Task DeleteAllStudentInGroup(int groupId);
        Task UpdateStudentGroup(int studentId, int classId, int groupNumber);
        Task<int> GetCurrentNumberOfMemberInGroup(int groupId);
        Task AddStudentInGroup(int studentId, int groupId, int classId, int isLeader);
        Task<int> FindStudentLeaderRoleInClass(int studentId, int classId);
        Task<int> FindLeaderInGroup(int groupId);
        Task<int> IsStudentExistInGroup(int groupId, int studentId);
        Task UpdateGroupLeader(int groupId, int studentId, int isLeader);
        Task<int> ChooseRandomGroupMember(int groupId);
        Task AddRandomGroupLeader(int groupId, int leaderId);
        Task<int> FindGroupIdByLeader(int leaderId);
    }
}
