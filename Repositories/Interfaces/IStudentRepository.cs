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

        Task<Group> FindGroupByStudentIdAndClassId(int studentId, int classId);
        Task<int> FindStudentIdByEmail(string email);
        Task<Student> FindOneById(int studentId);
        Task<bool> ExistsByEmail(string email);
    }
}
