using BusinessObjects.Models;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ISemesterRepository
    {
        public Task<HashSet<Semester>> GetSemester(String code);

         public Task<DateTime> GetSemesterEndDate(String code);

         public Task<DateTime> GetSemesterStartDate(String code);
    }
}
