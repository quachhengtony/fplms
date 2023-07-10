using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private static SubjectRepository? instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext? dbContext;

        public static SubjectRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new SubjectRepository();
                    }
                    return instance;
                }
            }
        }
        public async Task<bool> ExistsByName(string name)
        {
            return await dbContext.Subjects.AnyAsync(s => s.Name == name);
        }

        public async Task<Subject> FindByName(string subjectName)
        {
            return await dbContext.Subjects.FirstOrDefaultAsync(s => s.Name == subjectName);
        }

        public async Task<bool> ExistsById(int subjectId)
        {
            return await dbContext.Subjects.AnyAsync(s => s.Id == subjectId);
        }
    }
}
