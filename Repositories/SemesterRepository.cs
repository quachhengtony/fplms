using BusinessObjects.DbContexts;
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
    public class SemesterRepository :ISemesterRepository
    {
        private static SemesterRepository? instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext? dbContext;

        public static SemesterRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new SemesterRepository();
                    }
                    return instance;
                }
            }
        }

        public async Task<HashSet<Semester>> GetSemester(string code)
        {
            var semesters = await dbContext.Semesters.FromSqlRaw("SELECT * FROM SEMESTER WHERE code LIKE {0}", code).ToListAsync();
            return semesters.ToHashSet();
        }

        public async Task<DateTime> GetSemesterEndDate(string code)
        {
            return (DateTime)await dbContext.Semesters.Where(s => s.Code == code).Select(s => s.EndDate).FirstOrDefaultAsync();
        }

        public async Task<DateTime> GetSemesterStartDate(string code)
        {
            return (DateTime)await dbContext.Semesters.Where(s => s.Code == code).Select(s => s.StartDate).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsById(string code)
        {
            return await dbContext.Semesters.AnyAsync(s => s.Code == code);
        }
    }
}
