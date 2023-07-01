using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using BusinessObjects.DbContexts;
using Repositories.Interfaces;

namespace Repositories
{
    public class CycleReportRepository : ICycleReportRepository
    {
        private static CycleReportRepository instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext dbContext;

        public static CycleReportRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new CycleReportRepository();
                    }
                    return instance;
                }
            }
        }

        public Task<int> AddFeedbackAsync(int reportId, string feedback, float mark)
        {
            return dbContext.Database.ExecuteSqlRawAsync($"update cycle_report set feedback = '{feedback}', mark = {mark} where id = {reportId}");
        }

        public Task<int> ExistsByGroupAndCycleNumberAsync(Group group, int cycleNumber)
        {
            return dbContext.CycleReports.Where(c => c.GroupId == group.Id && c.CycleNumber == cycleNumber)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public Task<int> ExistsByIdAndGroupIdAsync(int groupId, int reportId)
        {
            return dbContext.CycleReports.Where(c => c.Id == reportId && c.GroupId == groupId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public Task<List<CycleReport>> FindByGroupAsync(Group group)
        {
            return dbContext.CycleReports.Where(r => r.GroupId == group.Id).ToListAsync();
        }
    }
}
