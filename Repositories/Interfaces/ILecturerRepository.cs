using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface ILecturerRepository
    {
        public Task<bool> ExistsByEmailAsync(string lecturerEmail);

        public Task<Lecturer> FindOneByEmailAsync(string lecturerEmail);

        public Task<int> FindLecturerIdByEmailAsync(string lecturerEmail);
    }
}
