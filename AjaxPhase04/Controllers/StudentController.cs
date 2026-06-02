using Microsoft.AspNetCore.Mvc;
using AjaxPhase04.Models;
namespace AjaxPhase04.Controllers
{
    public class StudentController : Controller
    {
        static List<Student> students = new List<Student>();
        public IActionResult Index()
        {
            return View(students);
        }

        //[HttpGet]
        //public JsonResult GetStudents()

        //{
        //    return Json(students);
        //}
        [HttpGet]
       
        public JsonResult GetStudentById(int id)
        {
            var student = students.FirstOrDefault(x => x.Id == id);

            if (student == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Student not found"
                });
            }

            return Json(new
            {
                success = true,
                student = student
            });
        }
        [HttpPost]
        
        public JsonResult GetStudentsServerSide()
        {
            int draw = Convert.ToInt32(Request.Form["draw"]);
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            string departmentFilter =Request.Form["columns[3][search][value]"];

            var query = students.AsQueryable();

            int totalRecords = query.Count();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                    x.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                    x.Email.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                    x.Department.Contains(searchValue, StringComparison.OrdinalIgnoreCase)
                );
            }
            if (!string.IsNullOrEmpty(departmentFilter))
            {
                query = query.Where(x => x.Department == departmentFilter);
            }

            int filteredRecords = query.Count();

            var data = query
                .Skip(start)
                .Take(length)
                .ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = data
            });
        }
        [HttpPost]
       
        public JsonResult AddStudent(Student student)
        {
            student.Id = students.Count + 1;
            students.Add(student);

            return Json(new
            {
                success = true,
                message="Student added successfully",
                student= student
            });

        }
        [HttpPost]
        
        public JsonResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(x => x.Id == id);

            if (student != null)
            {
                students.Remove(student);
            }

            return Json(new
            {
                success = true
            });
        }
        [HttpPost]
        
        public JsonResult UpdateStudent(Student updatedStudent)
        {
            var student = students.FirstOrDefault(x => x.Id == updatedStudent.Id);

            if (student != null)
            {
                student.Name = updatedStudent.Name;
                student.Email = updatedStudent.Email;
                student.Department = updatedStudent.Department;
            }

            return Json(new
            {
                success = true,
                message = "Student updated successfully"
            });
        }
        public IActionResult CreateNormal()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNormal(Student student)
        {
            student.Id = students.Count + 1;
            students.Add(student);

            return RedirectToAction("Index");
        }

    }
}
