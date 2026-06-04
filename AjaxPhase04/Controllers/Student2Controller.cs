using Microsoft.AspNetCore.Mvc;
using AjaxPhase04.Models;
using System.Collections.Generic;
using System.Linq;

namespace AjaxPhase04.Controllers
{
    public class Student2Controller : Controller
    {
        private static readonly List<Student> _students = new List<Student>
        {
            new Student { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Department = "SE" },
            new Student { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Department = "CS" },
            new Student { Id = 3, Name = "Charlie Davis", Email = "charlie@example.com", Department = "IT" },
            new Student { Id = 4, Name = "Diana Prince", Email = "diana@example.com", Department = "DS" },
            new Student { Id = 5, Name = "Evan Wright", Email = "evan@example.com", Department = "SE" }
        };

        public IActionResult Index()
        {
            return View(_students);
        }

        [HttpGet]
        public JsonResult GetStudentById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }
            return Json(new { success = true, student });
        }

        [HttpPost]
        public JsonResult AddStudent(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name))
            {
                return Json(new { success = false, message = "Invalid student data" });
            }

            student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
            _students.Add(student);

            return Json(new { success = true, message = "Student added successfully to Student2 list", student });
        }

        [HttpPost]
        public JsonResult UpdateStudent(Student updatedStudent)
        {
            if (updatedStudent == null)
            {
                return Json(new { success = false, message = "Invalid student data" });
            }

            var student = _students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            if (student == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;
            student.Department = updatedStudent.Department;

            return Json(new { success = true, message = "Student updated successfully in Student2 list" });
        }

        [HttpPost]
        public JsonResult DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            _students.Remove(student);
            return Json(new { success = true, message = "Student deleted successfully from Student2 list" });
        }

        [HttpGet]
        public JsonResult GetDepartmentStats()
        {
            var stats = _students
                .GroupBy(s => s.Department)
                .Select(g => new { Department = g.Key, Count = g.Count() })
                .ToList();

            return Json(stats);
        }
    }
}
