using BusinessObjects.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IProgressReportRepository
    {
        Task<HashSet<ProgressReport>> FindByGroup(Group group);

        Task<int> ExistsByIdAndGroupIdAndStudentId(int groupId, int reportId, int studentId);

        Task<int> ExistsByStudentIdAndGroupIdAndCurDate(int studentId, int groupId, DateTime curDate);

        Task<HashSet<ProgressReport>> FindByGroupIdAndTimeFilter(int groupId, DateTime startDate, DateTime endDate);

        Task<DateTime> GetDateOfProgressReport(int reportId);
        Task<bool> ExistsById(int reportId);
        Task<ProgressReport> GetByIdAsync(int reportId);
        Task SaveAsync(ProgressReport progressReport);
        Task DeleteAsync(int reportId);
    }
}
