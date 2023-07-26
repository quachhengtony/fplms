using System;
using System.Threading.Tasks;
using Repositories.Contracts;
using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class StudentUpvoteRepository : RepositoryBase<StudentUpvote>, IStudentUpvoteRepository
    {
        public StudentUpvoteRepository(FplmsManagementContext repositoryContext) : base(repositoryContext)
        {

        }

        public async void CreateStudentUpvote(StudentUpvote studentUpvote)
        {
            Create(studentUpvote);
        }

        public void DeleteStudentUpvote(StudentUpvote studentUpvote)
        {
            Delete(studentUpvote);
        }

        public async Task<StudentUpvote> GetStudentUpvote(StudentUpvote dto)
        {
            return await FindByCondition(studentUpvote => studentUpvote.QuestionId == dto.QuestionId && studentUpvote.StudentId == dto.StudentId).FirstOrDefaultAsync();
        }

        public void UpdateStudentUpvote(StudentUpvote studentUpvote)
        {
            throw new NotImplementedException();
        }
    }
}