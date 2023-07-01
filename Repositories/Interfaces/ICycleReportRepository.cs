﻿using System;
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

        public Task<int> ExistsByGroupAndCycleNumberAsync(Group group, int cycleNumber);

        public Task<int> AddFeedbackAsync(int reportId, string feedback, float mark);
    }
}