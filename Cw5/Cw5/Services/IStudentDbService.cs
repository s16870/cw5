using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DTO.Requests;
using Cw5.DTO.Responses;
using Cw5.Models;

namespace Cw5.Services
{
    public interface IStudentDbService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request);
        Student GetStudent(String id);
        IEnumerable<Student> GetStudents();
    }
}
