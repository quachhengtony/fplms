using System;
using System.Threading.Tasks;
using Repositories.Contracts;
using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class StudentAnswerUpvoteRepository : RepositoryBase<StudentAnswerUpvote>, IStudentAnswerUpvoteRepository
    {
        public StudentAnswerUpvoteRepository(FplmsManagementContext repositoryContext) : base(repositoryContext)
        {

        }

        public async void CreateStudentAnswerUpvote(StudentAnswerUpvote studentUpvote)
        {
            Create(studentUpvote);
        }

        public void DeleteStudentAnswerUpvote(StudentAnswerUpvote studentUpvote)
        {
            Delete(studentUpvote);
        }

        public async Task<StudentAnswerUpvote> GetStudentAnswerUpvote(StudentAnswerUpvote dto)
        {
            return await FindByCondition(studentUpvote => studentUpvote.AnswerId == dto.AnswerId && studentUpvote.StudentId == dto.StudentId).FirstOrDefaultAsync();
        }

        public void UpdateStudentAnswerUpvote(StudentAnswerUpvote studentUpvote)
        {
            throw new NotImplementedException();
        }
    }
}