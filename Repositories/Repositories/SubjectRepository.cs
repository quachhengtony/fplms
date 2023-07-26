using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Contracts;
using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository(FplmsManagementContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateSubject(Subject subject)
        {
            Create(subject);
        }

        public void DeleteSubject(Subject subject)
        {
            Delete(subject);
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(Guid subjectId)
        {
            return await FindByCondition(subject => subject.Id.Equals(subjectId)).FirstOrDefaultAsync();
        }

        public async Task<Subject> GetSubjectByNameAsync(string subjectName)
        {
            return await FindByCondition(subject => subject.Name.ToLower().Equals(subjectName.Trim().ToLower())).FirstOrDefaultAsync();
        }

        public void UpdateSubject(Subject subject)
        {
            Delete(subject);
        }
    }
}