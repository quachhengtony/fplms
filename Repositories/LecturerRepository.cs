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
    public class LecturerRepository : ILecturerRepository
    {
        private static LecturerRepository instance;
        private static readonly object instanceLock = new object();
        private static FplmsManagementContext dbContext;

        public static LecturerRepository Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        dbContext = new FplmsManagementContext();
                        instance = new LecturerRepository();
                    }
                    return instance;
                }
            }
        }

        public Task<int> ExistsByEmailAsync(string lecturerEmail)
        {
            return dbContext.Lecturers.Where(l => l.Email == lecturerEmail).Select(l => l.Id).FirstOrDefaultAsync();
        }

        public Task<int> FindLecturerIdByEmailAsync(string lecturerEmail)
        {
            return dbContext.Lecturers.Where(l => l.Email == lecturerEmail).Select(l => l.Id).FirstOrDefaultAsync();
        }

        public Task<Lecturer> FindOneByEmailAsync(string lecturerEmail)
        {
            return dbContext.Lecturers
                .Include(l => l.Classes)
                .Where(l => l.Email == lecturerEmail).FirstOrDefaultAsync();
        }

        public Task SaveLecturerAsync(Lecturer lecturer)
        {
            dbContext.Lecturers.Add(lecturer);
            return dbContext.SaveChangesAsync();
        }
    }
}
