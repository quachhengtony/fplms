using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using BusinessObjects.DbContexts;
using Repositories.Interfaces;
using MySql.Data.MySqlClient;

namespace Repositories
{
    public class CycleReportRepository : ICycleReportRepository
    {


        private FplmsManagementContext dbContext;

        public CycleReportRepository()
        {
            dbContext = new FplmsManagementContext();
        }

        public Task<int> AddFeedbackAsync(int reportId, string feedback, float mark)
        {
            return dbContext.Database.ExecuteSqlRawAsync("UPDATE cycle_report SET feedback = @feedback, mark = @mark WHERE id = @reportId",
        new MySqlParameter("@feedback", feedback),
        new MySqlParameter("@mark", mark),
        new MySqlParameter("@reportId", reportId));
        }

        public Task<int> ExistsByGroupAndCycleNumberAsync(int groupId, int cycleNumber)
        {
            return dbContext.CycleReports.Where(c => c.GroupId == groupId && c.CycleNumber == cycleNumber)
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

        public async Task<bool> ExistsById(int reportId)
        {
            return await dbContext.CycleReports
                .AnyAsync(c => c.Id == reportId);
        }

        public async Task<CycleReport> GetByIdAsync(int reportId)
        {
            return await dbContext.CycleReports.FindAsync(reportId);
        }

        public async Task<CycleReport> SaveAsync(CycleReport cycleReport)
        {
            dbContext.CycleReports.Add(cycleReport);
            await dbContext.SaveChangesAsync();
            return cycleReport;
        }

        public async Task<CycleReport> GetByIdAndGroupIdAsync(int groupId, int reportId)
        {
            return await dbContext.CycleReports
                .FirstOrDefaultAsync(c => c.Id == reportId && c.GroupId == groupId);
        }

        public async Task DeleteAsync(CycleReport cycleReport)
        {
            dbContext.CycleReports.Remove(cycleReport);
            await dbContext.SaveChangesAsync();
        }


    }
}
