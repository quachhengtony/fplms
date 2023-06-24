using BusinessObjects.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        Task<bool> ExistsByName(string name);
        Task<Subject> FindByName(string subjectName);
    }
}
