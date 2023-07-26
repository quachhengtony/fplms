using Repositories.Contracts;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.DbContexts;
using Repositories.Helpers;
using Repositories.Enum;
using Repositories.Dto.Temp;

namespace Api.Repositories
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(FplmsManagementContext repositoryContext) : base(repositoryContext)
        {

        }

        public void CreateQuestion(Question question)
        {
            Create(question);
        }

        public void DeleteQuestion(Question question)
        {
            Delete(question);
        }

        // public async Task<PagedList<Question>> GetAllQuestionsAsync(QuestionsQueryStringParameters queryStringParameters)
        // {

        //     var items = await FindAll()
        //                         .Where(question => question.Removed == false)
        //                         // .OrderByDescending(question => question.CreatedDate)
        //                         // .Skip((queryStringParameters.PageNumber - 1) * queryStringParameters.PageSize)
        //                         // .Take(queryStringParameters.PageSize)
        //                         .Include(question => question.Student)
        //                         .Include(question => question.Subject)
        //                         .Include(question => question.Upvoters)
        //                         .Include(question => question.Answers)
        //                         .ToListAsync();

        //     if (!string.IsNullOrWhiteSpace(queryStringParameters.Question))
        //     {
        //         items = items.Where(question =>
        //                         question.Title.ToLower().Contains(queryStringParameters.Question.Trim().ToLower())
        //                         || question.Content.ToLower().Contains(queryStringParameters.Question.Trim().ToLower()))
        //                         .ToList();
        //     }

        //     if (!string.IsNullOrWhiteSpace(queryStringParameters.Subject))
        //     {
        //         items = items.Where(question =>
        //                                 question.Subject.Name.Equals(queryStringParameters.Subject))
        //                                 .ToList();
        //     }


        //     if (queryStringParameters.Sort == SortType.NEW)
        //     {
        //         items = items.OrderByDescending(question => question.CreatedDate).ToList();
        //     }

        //     if (queryStringParameters.Sort == SortType.TOP)
        //     {
        //         items = items.OrderByDescending(question => question.Upvoters.Count).ToList();
        //     }

        //     if (queryStringParameters.Sort == SortType.HOT)
        //     {
        //         items = items.OrderByDescending(question => question.CreatedDate).OrderByDescending(question => question.Upvoters.Count).ToList();
        //     }

        //     return PagedList<Question>.ToPagedList(items, queryStringParameters.PageNumber, queryStringParameters.PageSize);
        // }

        public async Task<PagedList<Question>> GetAllQuestionsAsync(string? question, string? subject, SortType sortType, int pageNumber, int pageSize)
        {

            var items = await FindAll()
                                .Where(question => question.Removed == false)
                                // .OrderByDescending(question => question.CreatedDate)
                                // .Skip((queryStringParameters.PageNumber - 1) * queryStringParameters.PageSize)
                                // .Take(queryStringParameters.PageSize)
                                .Include(question => question.Student)
                                .Include(question => question.Subject)
                                .Include(question => question.Upvoters)
                                .Include(question => question.Answers)
                                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(question))
            {
                items = items.Where(q =>
                                q.Title.ToLower().Contains(question.Trim().ToLower())
                                || q.Content.ToLower().Contains(question.Trim().ToLower()))
                                .ToList();
            }

            if (!string.IsNullOrWhiteSpace(subject))
            {
                items = items.Where(question =>
                                        question.Subject.Name.Equals(subject))
                                        .ToList();
            }


            if (sortType == SortType.NEW)
            {
                items = items.OrderByDescending(question => question.CreatedDate).ToList();
            }

            if (sortType == SortType.TOP)
            {
                items = items.OrderByDescending(question => question.Upvoters.Count).ToList();
            }

            if (sortType == SortType.HOT)
            {
                items = items.OrderByDescending(question => question.CreatedDate).OrderByDescending(question => question.Upvoters.Count).ToList();
            }

            return PagedList<Question>.ToPagedList(items, pageNumber, pageSize);
        }

        public async Task<Question> GetQuestionByIdAsync(Guid questionId, string mode = "")
        {
            if (mode == "eager")
            {
                return await FindAll()
                                .Where(question => question.Id.Equals(questionId))
                                .Include(question => question.Student)
                                .Include(question => question.Subject)
                                .Include(question => question.Upvoters)
                                .Include(question => question.Answers.Where(answer => answer.Removed == false))
                                .ThenInclude(answer => answer.Upvoters)
                                .Include(question => question.Answers.Where(answer => answer.Removed == false))
                                .ThenInclude(answer => answer.Student)
                                .FirstOrDefaultAsync();
            }
            return await FindByCondition(question => question.Id.Equals(questionId))
                        .Include(question => question.Subject)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByStudentId(int studentId)
        {
            return await FindByCondition(question => question.StudentId.Equals(studentId))
                            .Include(question => question.Subject)
                            .Include(question => question.Student)
                            .OrderByDescending(question => question.CreatedDate)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsRemovedByLecturer(string lecturerEmail)
        {
            // return await FindByCondition(question => question.RemovedBy.Equals(lecturerEmail))
            //                 .ToListAsync();
            return await FindAll()
                            .Where(question => question.RemovedBy.Equals(lecturerEmail))
                            .Include(question => question.Subject)
                            .Include(question => question.Student)
                            .Include(question => question.Answers)
                            .ThenInclude(answer => answer.Student)
                            .ToListAsync();
        }

        public async Task<Question> GetQuestionByAnswerId(int studentId, Guid answerId)
        {
            var question = await FindAll()
                                .Where(question => question.StudentId == studentId && question.Removed == false)
                                .Include(question => question.Answers.Where(a => a.Id == answerId))
                                .FirstOrDefaultAsync();
            return question;
        }

        public void UpdateQuestion(Question question)
        {
            Update(question);
        }
    }
}