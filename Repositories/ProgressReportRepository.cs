﻿using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProgressReportRepository : IProgressReportRepository
    {
        private static ProgressReportRepository? instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext? dbContext;

        public static ProgressReportRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new ProgressReportRepository();
                    }
                    return instance;
                }
            }
        }
        public async Task<HashSet<ProgressReport>> FindByGroup(Group group)
        {
            var reports = await dbContext.ProgressReports
                .Where(pr => pr.Group == group)
                .ToListAsync();
            return reports.ToHashSet();
        }


        public async Task<int> ExistsByIdAndGroupIdAndStudentId(int groupId, int reportId, int studentId)
        {
            return await dbContext.ProgressReports
                .Where(pr => pr.GroupId == groupId && pr.Id == reportId && pr.StudentId == studentId)
                .Select(pr => pr.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> ExistsByStudentIdAndGroupIdAndCurDate(int studentId, int groupId, DateTime curDate)
        {
            return await dbContext.ProgressReports
                .Where(pr => pr.StudentId == studentId && pr.GroupId == groupId && pr.ReportTime == curDate)
                .Select(pr => pr.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<HashSet<ProgressReport>> FindByGroupIdAndTimeFilter(int groupId, DateTime startDate, DateTime endDate)
        {
            var reports  = await dbContext.ProgressReports
                .Where(pr => pr.GroupId == groupId && pr.ReportTime >= startDate && pr.ReportTime <= endDate)
                .ToListAsync();
            return reports.ToHashSet();
        }

        public async Task<DateTime> GetDateOfProgressReport(int reportId)
        {
            return await dbContext.ProgressReports
                .Where(pr => pr.Id == reportId)
                .Select(pr => pr.ReportTime)
                .FirstOrDefaultAsync();
        }
    }
}
