using BusinessLogicLayer;  // BLLApplication için
using EntityLayer; // Entity modellerini kullanmak için
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

public class ApplicationController : Controller
{
    // Öğrenci ve ders ilişkilerini getir
    public IActionResult StudentCourses()
    {
        var applications = BLLApplication.GetApplications();  // BLL'den öğrenci-ders ilişkilerini alıyoruz
        return View("GetStudentCourses", applications);  // Görünüm doğru olmalı
    }

    // Yeni öğrenciye ders eklemek için sayfa (GET)
    [HttpGet]
    public IActionResult AddCourse()
    {
        // Öğrencileri ve kursları alıyoruz
        var students = BLLApplication.GetStudents();
        var courses = BLLApplication.GetCourses();

        // Eğer öğrenci verisi varsa, ViewBag'e atıyoruz
        ViewBag.Students = students != null && students.Any()
            ? new SelectList(students, "StudentID", "StudentName")
            : new SelectList(Enumerable.Empty<SelectListItem>());

        // Eğer kurs verisi varsa, ViewBag'e atıyoruz
        ViewBag.Courses = courses != null && courses.Any()
            ? new SelectList(courses, "CourseID", "CourseName")
            : new SelectList(Enumerable.Empty<SelectListItem>());

        return View();
    }

    [HttpPost]
    public IActionResult AddCourse(int studentId, int courseId)
    {
        var result = BLLApplication.AddApplication(studentId, courseId);

        if (result > 0)
        {
            TempData["Success"] = "Course successfully added!";
        }
        else
        {
            TempData["Error"] = "An error occurred while adding the course.";
        }

        return RedirectToAction("StudentCourses");
    }

    // Edit GET metodu: Ders bilgilerini ve öğrenci bilgilerini almak için
    [HttpGet]
    public IActionResult Edit(int studentId)
    {
        var student = BLLApplication.GetStudentById(studentId); // Öğrenciyi ID ile getiriyoruz
        if (student == null)
        {
            return NotFound();  // Eğer öğrenci bulunamazsa, 404 döndürüyoruz
        }

        // Dersleri ViewBag'e ekliyoruz
        ViewBag.Courses = new SelectList(BLLApplication.GetCourses(), "CourseID", "CourseName");

        return View(student);  // Öğrenci ve ders bilgilerini gönderiyoruz
    }

    // Edit POST metodu: Öğrenci ve ders bilgisini güncellemek için
    [HttpPost]
    public IActionResult Edit(int studentId, int courseId)
    {
        var result = BLLApplication.UpdateApplication(studentId, courseId);  // Öğrenci ve ders bilgilerini güncelleme işlemi

        if (result > 0)
        {
            TempData["Success"] = "Student's course updated successfully!";
            return RedirectToAction("StudentCourses");  // Sayfayı tekrar yükler
        }

        TempData["Error"] = "An error occurred while updating the student's course.";
        return View();
    }

    // Delete metodu: Öğrenci-ders ilişkisinin silinmesi
    public IActionResult Delete(int studentId, int courseId)
    {
        var result = BLLApplication.DeleteApplication(studentId, courseId);  // Öğrenci ve ders ilişkisini sil

        if (result > 0)
        {
            TempData["Success"] = "Student successfully removed from the course!";
        }
        else
        {
            TempData["Error"] = "An error occurred while deleting the student course.";
        }

        return RedirectToAction("StudentCourses");  // Sayfayı tekrar yükler
    }
}
