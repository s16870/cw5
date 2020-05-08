using Cw5.Models;
using System.Collections.Generic;

namespace Cw5.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}
