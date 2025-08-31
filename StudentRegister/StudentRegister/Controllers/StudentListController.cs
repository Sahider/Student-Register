using EntityLayer;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


namespace StudentRegister.Controllers
{
    public class StudentListController : Controller
    {
        // Öğrenci listesi sayfası
        public IActionResult Index()
        {
            List<EntityStudent> ogrenciler = BLLStudent.GetStudentListBLL();
            return View(ogrenciler);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            EntityStudent student = BLLStudent.GetStudentByIdBLL(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EntityStudent model)

        {
            Regex namePattern = new Regex("^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$");

            if (!namePattern.IsMatch(model.Name) || !namePattern.IsMatch(model.Surname))
            {
                ModelState.AddModelError("", "Name and Surname must only contain letters and spaces.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                bool result = BLLStudent.UpdateStudentBLL(model);
                if (result)
                {
                    return RedirectToAction("Index"); // Başarılıysa listeye dön
                }
                else
                {
                    ModelState.AddModelError("", "Güncelleme başarısız oldu.");
                }
            }
            return View(model);
        }


        // Silme Onay Sayfası (GET)
        public IActionResult Delete(int id)
        {
            var student = BLLStudent.GetStudentByIdBLL(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // Silme işlemi (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            bool result = BLLStudent.DeleteStudentBLL(id); 

            if (result)
            {
                TempData["Message"] = "Student deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "Delete failed.";
            return RedirectToAction("Index");
        }

        
       
    }
}