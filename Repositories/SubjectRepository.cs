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
        private FplmsManagementContext dbContext;

        public SubjectRepository()
        {
            dbContext = new FplmsManagementContext();
        }
        public async Task<bool> ExistsByName(string name)
        {
            return await dbContext.Subjects.AnyAsync(s => s.Name == name);
        }

        public async Task<Subject> FindByName(string subjectName)
        {
            return await dbContext.Subjects.Include(s => s.Classes).FirstOrDefaultAsync(s => s.Name == subjectName);
        }

        public async Task<Subject> FindById(int id)
        {
            return await dbContext.Subjects.FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<List<Subject>> FindAll()
        {
            return await dbContext.Subjects.ToListAsync();
        }

        public async Task Save(Subject subject)
        {
            await dbContext.Subjects.AddAsync(subject);
            dbContext.SaveChanges();
            return;
        }

        public async Task Delete(Subject subject)
        {
            dbContext.Subjects.Remove(subject);
            dbContext.SaveChanges();
            return;
        }

        public async Task<bool> ExistsById(int id)
        {
            return await dbContext.Subjects.AnyAsync(s => s.Id == id);
        }

        public async Task Update(Subject subject)
        {
            dbContext.Subjects.Update(subject);
            await dbContext.SaveChangesAsync();
        }


    }
}
