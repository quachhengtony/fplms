using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Helpers;
using BusinessObjects.Models;
using Repositories.Dto.Temp;

namespace Repositories.Contracts
{
    public interface IAnswerRepository : IRepositoryBase<Answer>
    {
        Task<PagedList<Answer>> GetAllAnswersAsync(AnswersQueryStringParameters answersQueryStringParameters);
        Task<Answer> GetAnswerByIdAsync(Guid answerId);
        Task<IEnumerable<Answer>> GetAnswersByStudentId(int studentId);
        Task<IEnumerable<Answer>> GetAnswersByQuestionId(Guid questionId);
        Task<IEnumerable<Answer>> GetAnswersRemovedByLecturer(String lecturerEmail);
        void CreateAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
    }
}