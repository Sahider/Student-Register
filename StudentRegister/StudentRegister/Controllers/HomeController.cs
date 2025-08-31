using BusinessLogicLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentRegister.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace StudentRegister.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Anasayfa  (Views/Home/Index.cshtml )
        public IActionResult Index()
        {
            return View(); // View => Index.cshtml
        }

        // Gizlilik  (Views/Home/Privacy.cshtml)
        public IActionResult Privacy()
        {
            return View();
        }

        // About sayfasý (Views/Home/About.cshtml)
        public IActionResult About()
        {
            return View();
        }
        // Help Sayfasý (Views/Home/Help.cshtml)
        public IActionResult Help()
        {
            return View();
        }
        public IActionResult Security()
        {
            return View();
        }

        // Dersler sayfasýna yönlendirme
        public IActionResult Courses()
        {
            return RedirectToAction("Courses", "Course"); 
        }

        // Türkçe karakterleri kaldýrýr (e-posta için)
        private string RemoveTurkishCharacters(string input)
        {
            string[,] replacements = new string[,]
            {
                {"ç", "c"}, {"Ç", "C"},
                {"ð", "g"}, {"Ð", "G"},
                {"ý", "i"}, {"Ý", "I"},
                {"ö", "o"}, {"Ö", "O"},
                {"þ", "s"}, {"Þ", "S"},
                {"ü", "u"}, {"Ü", "U"}
            };

            for (int i = 0; i < replacements.GetLength(0); i++)
            {
                input = input.Replace(replacements[i, 0], replacements[i, 1]);
            }

            return input.ToLower(); // Küçük harfe çevir
        }

        // Ýlk harfi büyük yapar (Ýsim düzenleme için)
        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            input = input.ToLower();
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        //Gelen öðrenci verisini iþler
        [HttpPost]
        public IActionResult Submit(EntityStudent student, string EmailUserPart)
        {
            Regex namePattern = new Regex("^[a-zA-ZðüþýöçÐÜÞÝÖÇ ]+$");
            Regex numberPattern = new Regex("^[0-9]+$");

            if (string.IsNullOrEmpty(student.Kimlik) || !numberPattern.IsMatch(student.Kimlik))
            {
                ModelState.AddModelError("Kimlik", "Identity must contain only numbers.");
            }

            if (string.IsNullOrEmpty(student.Name) || !namePattern.IsMatch(student.Name))
            {
                ModelState.AddModelError("Name", "Name must contain only letters and spaces.");
            }

            if (string.IsNullOrEmpty(student.Surname) || !namePattern.IsMatch(student.Surname))
            {
                ModelState.AddModelError("Surname", "Surname must contain only letters and spaces.");
            }

            if (string.IsNullOrEmpty(student.Number) || !numberPattern.IsMatch(student.Number))
            {
                ModelState.AddModelError("Number", "Number must be numeric.");
            }

            // Model doðrulama baþarýlýysa
            if (ModelState.IsValid)
            {
                // Ayný kimlik numarasý var mý kontrolü
                if (BLLStudent.StudentExistsByKimlik(student.Kimlik))
                {
                    
                    ModelState.AddModelError("Kimlik", "This identity number is already registered.");
                    ViewBag.Mesaj = "This identity number is already registered.";
                    return View("Index", student); // Index view'ýna mevcut veriyle geri dön
                }

                // Ýsim ve soyadý formatla
                student.Name = CapitalizeFirstLetter(student.Name);
                student.Surname = CapitalizeFirstLetter(student.Surname);

                if (!string.IsNullOrEmpty(EmailUserPart))
                {
                    // E-posta kullanýcý adýný türkçe karakterden kurtarma iþlemi
                    EmailUserPart = RemoveTurkishCharacters(EmailUserPart).ToLower();
                    student.Email = EmailUserPart + "@lessons.com";
                }
                else
                {
                    ModelState.AddModelError("EmailUserPart", "Email username part is required.");
                    ViewBag.Mesaj = "Please enter a valid email username part!";
                    return View("Index", student);
                }

                // Öðrenciyi ekle
                int result = BLLStudent.AddStudentBLL(student);
                if (result > 0)
                {
                    TempData["SuccessMessage"] = "Student successfully added!";
                    return RedirectToAction("Index"); // Redirect ile form temizlenir ve mesaj görüntülenir
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occurred while adding student!";
                }
            }
            else
            {
                // Form hatalýysa
                ViewBag.Mesaj = "Please fill in all fields correctly!";
            }

            return View("Index", student);
        }

        // Hata sayfasý - sistemsel bir problem olursa çaðrýlýr
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
