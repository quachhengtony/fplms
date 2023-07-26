using Repositories.Helpers;
using BusinessObjects.Models;
using Repositories.Dto.Temp;
using Repositories.Enum;

namespace Repositories.Contracts
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        // Task<PagedList<Question>> GetAllQuestionsAsync(QuestionsQueryStringParameters queryStringParameters);
        Task<PagedList<Question>> GetAllQuestionsAsync(string question, string subject, SortType sortType, int pageNumber, int pageSize);
        Task<Question> GetQuestionByIdAsync(Guid questionId, string mode = "");
        Task<IEnumerable<Question>> GetQuestionsByStudentId(int studentId);
        Task<IEnumerable<Question>> GetQuestionsRemovedByLecturer(string lecturerEmail);
        Task<Question> GetQuestionByAnswerId(int studentId, Guid answerId);
        void CreateQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(Question question);
    }
}