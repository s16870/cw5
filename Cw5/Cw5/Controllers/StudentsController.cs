using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cw5.Models;
using Cw5.DAL;
using System.Data.SqlClient;
using Cw5.Services;

namespace Cw5.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private IStudentDbService _service;

        public StudentsController(IStudentDbService service)
        {
            _service = service;
        }

        public string GetStudent()
        {
            return "Kowalski, Malewski, Andrzejewski";
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            Student resp = _service.GetStudent(id);
            if(resp != null)
            {
                return Ok(resp);
            }
            else
            {
                return NotFound("Nie znaleziono studenta");
            }
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            var resp =_service.GetStudents();
            if(resp != null)
            {
                return Ok(resp);
            }
            else
            {
                return NotFound("Brak studentów");
            }
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