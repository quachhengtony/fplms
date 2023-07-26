// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Api.Dto.Temp;

// namespace Repositories.Contracts
// {
//     public interface IStudentsService
//     {
//         Task<List<GetStudentDto>> GetAllStudents();
//         Task<GetStudentDto> GetStudentById(Guid studentId);
//         Task<Guid> CreateStudent(CreateStudentDto createStudentDto);
//         Task<List<GetQuestionDto>> GetStudentQuestions(string userEmail);
//         Task<List<GetAnswerDto>> GetStudentAnswers(string userEmail);
//     }
// }