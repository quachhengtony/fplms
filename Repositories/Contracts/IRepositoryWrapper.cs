using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IQuestionRepository QuestionRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        IStudentRepository StudentRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        ILecturerRepository LecturerRepository { get; }
        IStudentUpvoteRepository StudentUpvoteRepository { get; }
        IStudentAnswerUpvoteRepository StudentAnswerUpvoteRepository { get; }
        Task SaveAsync();
    }
}