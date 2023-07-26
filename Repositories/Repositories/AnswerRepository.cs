using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Contracts;
using Repositories.Helpers;
using BusinessObjects.DbContexts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Dto.Temp;

namespace Api.Repositories
{
    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        public AnswerRepository(FplmsManagementContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateAnswer(Answer answer)
        {
            Create(answer);
        }

        public void DeleteAnswer(Answer answer)
        {
            Delete(answer);
        }

        public async Task<PagedList<Answer>> GetAllAnswersAsync(AnswersQueryStringParameters answersQueryStringParameters)
        {
            var items = await FindAll()
                                .Include(answer => answer.Student)
                                .OrderByDescending(answer => answer.CreatedDate)
                                .ToListAsync();

            return PagedList<Answer>.ToPagedList(items, answersQueryStringParameters.PageNumber, answersQueryStringParameters.PageSize);
        }

        public async Task<Answer> GetAnswerByIdAsync(Guid answerId)
        {
            return await FindByCondition(answer => answer.Id.Equals(answerId)).Include(answer => answer.Upvoters).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Answer>> GetAnswersByStudentId(int studentId)
        {
            return await FindByCondition(answer => answer.StudentId.Equals(studentId))
                            .Include(question => question.Student)
                            .OrderByDescending(answer => answer.CreatedDate)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionId(Guid questionId)
        {
            return await FindByCondition(answer => answer.QuestionId.Equals(questionId))
                            .ToListAsync();
        }

        public async Task<IEnumerable<Answer>> GetAnswersRemovedByLecturer(string lecturerEmail)
        {
            return await FindAll().Where(answer => answer.RemovedBy.Equals(lecturerEmail))
                            .Include(answer => answer.Student)
                            .Include(answer => answer.Question)
                            .ToListAsync();
        }

        public void UpdateAnswer(Answer answer)
        {
            Update(answer);
        }
    }
}