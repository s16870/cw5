using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cw5.Models;
using Cw5.DAL;
using System.Data.SqlClient;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private SqlConnection client;

        public StudentsController()
        {
            client =  new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s16870;Integrated Security=True");
        }

        public string GetStudent()
        {
            return "Kowalski, Malewski, Andrzejewski";
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            IActionResult res;
            var st = new Student();
            var com = new SqlCommand();
            com.Connection = client;
            com.CommandText = "SELECT * FROM StudentAPBD s LEFT JOIN ENROLLMENT e ON s.IdEnrollment = e.IdEnrollment LEFT JOIN STUDIES st on e.IdStudy = st.IdStudy WHERE IndexNumber LIKE @id";
            com.Parameters.AddWithValue("id", id);
            client.Open();
            var dr = com.ExecuteReader();
            if (dr.HasRows) { 
                while (dr.Read())
                {
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString());
                    st.Studies = dr["Name"].ToString();
                    st.Semester = Convert.ToInt32(dr["Semester"].ToString());

                }
                res = Ok(st);
            }else
            {
                res = NotFound("Nie znaleziono studenta");
            }
            com.Dispose();
            client.Dispose();
            return res;
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            var com = new SqlCommand();
            com.Connection = client;
            com.CommandText = "SELECT * FROM StudentAPBD s LEFT JOIN ENROLLMENT e ON s.IdEnrollment = e.IdEnrollment LEFT JOIN STUDIES st on e.IdStudy = st.IdStudy";
            client.Open();
            var dr = com.ExecuteReader();
            while (dr.Read())
            {
                var st = new Student();
                st.FirstName = dr["FirstName"].ToString();
                st.LastName = dr["LastName"].ToString();
                st.IndexNumber = dr["IndexNumber"].ToString();
                st.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString());
                st.Studies = dr["Name"].ToString();
                st.Semester = Convert.ToInt32(dr["Semester"].ToString());

                list.Add(st);
            }
            com.Dispose();
            client.Dispose();
            return Ok(list);
        }
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student)
        {
            return Ok($"Aktualizacja dokończona");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok($"Usuwanie ukończone");
        }

    }
}