using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface IMeetingRepository
    {
        public Task<List<Meeting>> FindByGroupIdAsync(int groupId, DateTime startDate, DateTime endDate);

        public Task<List<Meeting>> FindByClassIdAsync(int classId, DateTime startDate, DateTime endDate);

        public Task<List<Meeting>> FindByLecturerIdAsync(int lecturerId, DateTime startDate, DateTime endDate);

        public Task<List<Meeting>> FindByStudentIdAsync(int studentId, DateTime startDate, DateTime endDate);
        
        public Task<Meeting> FindOneByIdAsync(int meetingId);
        Task<bool> ExistsById(int meetingId);
        Task<int> AddAsync(Meeting meeting);
        Task<int> UpdateAsync(Meeting meeting);
        Task DeleteByIdAsync(int meetingId);
    }
}
