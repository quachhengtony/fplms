using System.Linq.Expressions;
using BusinessObjects.DbContexts;
using Repositories.Contracts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Api.Repositories
{
    public class LecturerRepository : RepositoryBase<Lecturer>, ILecturerRepository
    {
        public LecturerRepository(FplmsManagementContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateLecturer(Lecturer lecturer)
        {
            Create(lecturer);
        }

        public void DeleteLecturer(Lecturer lecturer)
        {
            Delete(lecturer);
        }

        public async Task<IEnumerable<Lecturer>> GetAllLecturersAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Lecturer> GetLecturerByIdAsync(Guid lecturerId)
        {
            return await FindByCondition(lecturer => lecturer.Id.Equals(lecturerId)).FirstOrDefaultAsync();
        }

        public async Task<Lecturer> GetLecturerByEmailAsync(string lecturerEmail)
        {
            return await FindByCondition(lecturer => lecturer.Email.Equals(lecturerEmail)).FirstOrDefaultAsync();
        }
        public void UpdateLecturer(Lecturer lecturer)
        {
            Update(lecturer);
        }
    }
}