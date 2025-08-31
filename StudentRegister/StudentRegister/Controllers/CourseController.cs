using BusinessLogicLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegister.Models;
using System.Linq;

namespace StudentRegister.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course List
        public IActionResult Index()
        {
            try
            {
                var courses = BLLCourse.GetCourseList() ?? new List<EntityCourse>();

                // Silinmiş dersleri session'dan al
                List<int> deletedCourseIds = HttpContext.Session.GetObjectFromJson<List<int>>("DeletedCourses") ?? new List<int>();

                // Silinmiş dersleri listeden çıkar
                courses = courses.Where(c => !deletedCourseIds.Contains(c.CourseID)).ToList();

                return View(courses);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Create Course Page
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CourseSelectionViewModel
            {
                Courses = BLLCourse.GetCourseList() ?? new List<EntityCourse>()
            };
            return View(model);
        }

        // POST: Create Course
        [HttpPost]
        public IActionResult Create(CourseSelectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var courseList = BLLCourse.GetCourseList() ?? new List<EntityCourse>();

                // Ders daha önce eklendi mi kontrol et
                bool alreadyExists = courseList.Any(c => c.CourseID == model.SelectedCourseID);
                if (alreadyExists)
                {
                    TempData["Error"] = "Bu ders zaten eklenmiş.";
                    model.Courses = courseList;
                    return View(model);
                }

                // Seçilen dersi bul
                var selected = courseList.FirstOrDefault(c => c.CourseID == model.SelectedCourseID);
                if (selected == null)
                {
                    TempData["Error"] = "Seçilen ders bulunamadı.";
                    model.Courses = courseList;
                    return View(model);
                }

                // Yeni kurs oluştur
                var newCourse = new EntityCourse
                {
                    CourseName = selected.CourseName,
                    MinCapacity = model.MinCapacity,
                    MaxCapacity = model.MaxCapacity
                };

                int result = BLLCourse.AddCourse(newCourse);

                if (result > 0)
                {
                    TempData["Success"] = "Ders başarıyla eklendi.";
                    return RedirectToAction("Index");
                }

                TempData["Error"] = "Ders eklenemedi.";
            }

            // Hatalıysa tekrar listeyi yükle
            model.Courses = BLLCourse.GetCourseList() ?? new List<EntityCourse>();
            return View(model);
        }

        // GET: Edit Course Page
        public IActionResult Edit(int id)
        {
            var course = BLLCourse.GetCourseById(id);
            if (course == null)
                return NotFound();

            var courses = BLLCourse.GetCourseList() ?? new List<EntityCourse>();

            ViewBag.CoursesSelectList = new SelectList(courses, "CourseID", "CourseName", course.CourseID);

            ViewBag.CoursesList = courses.Select(c => new
            {
                c.CourseID,
                c.MinCapacity,
                c.MaxCapacity,
                c.CourseName
            }).ToList();

            return View(course);
        }

        // POST: Edit Course
        [HttpPost]
        public IActionResult Edit(EntityCourse course)
        {
            if (ModelState.IsValid)
            {
                int result = BLLCourse.UpdateCourseBLL(course);
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Güncelleme başarısız oldu.");
            }

            return View(course);
        }

        // DELETE: Delete Course (sadece ekrandan sil)
        public IActionResult Delete(int id)
        {
            // Session'da silinmiş dersleri tutacağız
            List<int> deletedCourseIds = HttpContext.Session.GetObjectFromJson<List<int>>("DeletedCourses") ?? new List<int>();

            if (!deletedCourseIds.Contains(id))
            {
                deletedCourseIds.Add(id);
                HttpContext.Session.SetObjectAsJson("DeletedCourses", deletedCourseIds);
            }

            return RedirectToAction("Index");
        }
    }
}
