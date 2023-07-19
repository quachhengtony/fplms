using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface ICycleReportRepository
    {
        public Task<List<CycleReport>> FindByGroupAsync(Group group);

        public Task<int> ExistsByIdAndGroupIdAsync(int groupId, int reportId);

        public Task<int> ExistsByGroupAndCycleNumberAsync(int groupId, int cycleNumber);

        public Task<int> AddFeedbackAsync(int reportId, string feedback, float mark);
        Task<bool> ExistsById(int reportId);
        Task<CycleReport> GetByIdAsync(int reportId);
        Task<CycleReport> AddAsync(CycleReport cycleReport);
        Task<CycleReport> UpdateAsync(CycleReport cycleReport);
        Task<CycleReport> GetByIdAndGroupIdAsync(int groupId, int reportId);
        Task DeleteAsync(CycleReport cycleReport);
    }
}
