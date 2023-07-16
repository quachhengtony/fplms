using BusinessObjects.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IStudentRepository
    {

        public Task<Group> FindGroupByStudentIdAndClassId(int studentId, int classId);
        public Task<int> FindStudentIdByEmail(string email);
        public Task<Student> FindOneById(int studentId);
        public Task<bool> ExistsByEmail(string email);
        public Task<bool> ExistsById(int studentId);
    }
}
