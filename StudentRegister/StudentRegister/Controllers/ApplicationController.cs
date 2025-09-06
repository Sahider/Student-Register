using BusinessLogicLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

public class ApplicationController : Controller
{
    // Öğrenci ve ders ilişkilerini getir
    public IActionResult StudentCourses()
    {
        var applications = BLLApplication.GetApplications();
        return View("GetStudentCourses", applications);
    }

    // Öğrenciye yeni ders eklemek için sayfa (GET)
    [HttpGet]
    public IActionResult Edit(int studentId)
    {
        var student = BLLApplication.GetStudentById(studentId);
        if (student == null)
        {
            return NotFound();
        }


        var model = new StudentApplicationViewModel
        {
            StudentID = student.StudentID,
            StudentName = student.Name,        
            StudentSurname = student.Surname,  
            StudentNumber = student.Number,    
            StudentMail = student.Email        
        };


        ViewBag.Courses = new SelectList(BLLApplication.GetCourses(), "CourseID", "CourseName");

        return View(model);
    }


    // Öğrenciye yeni ders ekle (POST)
    [HttpPost]
    public IActionResult Edit(int studentId, int courseId)
    {
        var result = BLLApplication.AddApplication(studentId, courseId);

        if (result > 0)
        {
            TempData["Success"] = "Course successfully added to the student!";
            return RedirectToAction("StudentCourses");
        }

        TempData["Error"] = "This course is already assigned to the student.";

        var student = BLLApplication.GetStudentById(studentId);
        var model = new StudentApplicationViewModel
        {
            StudentID = student.StudentID,
            StudentName = student.Name,
            StudentSurname = student.Surname,
            StudentNumber = student.Number,
            StudentMail = student.Email
        };

        ViewBag.Courses = new SelectList(BLLApplication.GetCourses(), "CourseID", "CourseName");
        var studentCourses = BLLApplication.GetStudentCoursesByStudentId(studentId);
        ViewBag.StudentCourses = studentCourses;

        return View(model); 
    }


    // Delete metodu: Öğrenci-ders ilişkisinin silinmesi
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int studentId, int courseId)
    {
        var result = BLLApplication.DeleteApplication(studentId, courseId);

        if (result > 0)
        {
            TempData["Success"] = "Student successfully removed from the course!";
        }
        else
        {
            TempData["Error"] = "An error occurred while deleting the course.";
        }

        return RedirectToAction("StudentCourses");
    }

    [HttpGet]
    public IActionResult Delete(int studentId, int courseId)
    {
        var studentCourses = BLLApplication.GetStudentCoursesByStudentId(studentId);
        var model = studentCourses.FirstOrDefault(x => x.CourseID == courseId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model); 
    }
    // GET: AddCourse sayfasını göster (öğrenciler + dersler dropdown listesi)
    [HttpGet]
    public IActionResult AddCourse()
    {
        var students = BLLApplication.GetStudents()
                      .Select(s => new { s.StudentID, FullName = $"{s.Name} {s.Surname}" })
                      .ToList();

        ViewBag.Students = new SelectList(students, "StudentID", "FullName");
        ViewBag.Courses = new SelectList(BLLApplication.GetCourses(), "CourseID", "CourseName");
        return View();
    }


    // POST: Öğrenciye ders ekle
    [HttpPost]
    public IActionResult AddCourse(int studentId, int courseId)
    {
        var result = BLLApplication.AddApplication(studentId, courseId);

        if (result > 0)
        {
            TempData["Success"] = "Course successfully added to the student!";
            return RedirectToAction("StudentCourses");
        }

        TempData["Error"] = "This course is already assigned to the student.";

        var students = BLLApplication.GetStudents()
                      .Select(s => new { s.StudentID, FullName = $"{s.Name} {s.Surname}" })
                      .ToList();

        ViewBag.Students = new SelectList(students, "StudentID", "FullName", studentId);
        ViewBag.Courses = new SelectList(BLLApplication.GetCourses(), "CourseID", "CourseName", courseId);

        return View();
    }


    [HttpGet]
    public JsonResult GetStudentInfo(int studentId)
    {
        var student = BLLApplication.GetStudentById(studentId);
        if (student == null)
        {
            return Json(new { success = false });
        }

        return Json(new
        {
            success = true,
            name = student.Name,
            surname = student.Surname,
            number = student.Number
        });
    }

    [HttpGet]
    public JsonResult GetStudentCourses(int studentId)
    {
        var courses = BLLApplication.GetStudentCoursesByStudentId(studentId);
        if (courses == null || !courses.Any())
        {
            return Json(new { success = false, courses = new string[0] });
        }

        var courseNames = courses.Select(c => c.CourseName).ToArray();

        return Json(new { success = true, courses = courseNames });
    }


}
