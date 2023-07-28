using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Contracts
{
    public interface ILecturerRepository : IRepositoryBase<Lecturer>
    {
        Task<IEnumerable<Lecturer>> GetAllLecturersAsync();
        Task<Lecturer> GetLecturerByIdAsync(Guid lecturerId);
        Task<Lecturer> GetLecturerByEmailAsync(string lecturerEmail);
        void CreateLecturer(Lecturer lecturer);
        void UpdateLecturer(Lecturer lecturer);
        void DeleteLecturer(Lecturer lecturer);
    }
}