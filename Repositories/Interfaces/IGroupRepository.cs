using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface IGroupRepository
    {
        public Task Add(Group group);
        public Task Update(Group group);
        public Task Delete(Group group);
        public Task<Group> FindOneByIdAsync(int groupId);

        public Task<int> ExistByProjectAsync(int projectId);

        public Task<int> FindGroupByStudentIdAndClassIdAsync(int studentId, int classId);

        public Task<int> IsGroupExistsInClassAsync(int groupId, int classId);

        public Task<int> FindGroupNumberAsync(int groupId, int classId);

        public Task<int> GetGroupLimitNumberAsync(int groupId);

        public Task<int> GetMaxGroupNumberAsync(int classId);

        public Task<int> UpdateProjectInGroupAsync(int groupId, int projectId);

        public Task<int> IsEnrollTimeOverAsync(int groupId, DateTime currentTime);

        public Task<int> FindLectureIdOfGroupAsync(int groupId);

        public Task<int> IsGroupDisableAsync(int groupId);

        public Task<int> SetGroupDisableAsync(int groupId);

        public Task<int> SetGroupEnableAsync(int groupId);
        public Task<bool>  ExistsById(int groupId);
    }
}
