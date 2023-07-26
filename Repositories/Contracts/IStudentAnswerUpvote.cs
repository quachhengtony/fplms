using System.Threading.Tasks;
using BusinessObjects.Models;
using BusinessObjects.Models;

namespace Repositories.Contracts
{
    public interface IStudentAnswerUpvoteRepository : IRepositoryBase<StudentAnswerUpvote>
    {
        Task<StudentAnswerUpvote> GetStudentAnswerUpvote(StudentAnswerUpvote studentUpvote);
        void CreateStudentAnswerUpvote(StudentAnswerUpvote studentUpvote);
        void UpdateStudentAnswerUpvote(StudentAnswerUpvote studentUpvote);
        void DeleteStudentAnswerUpvote(StudentAnswerUpvote studentUpvote);
    }
}