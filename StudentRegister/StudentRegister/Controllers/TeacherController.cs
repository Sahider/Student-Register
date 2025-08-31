using BusinessLogicLayer;
using DataAccessLayer;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace StudentRegister.Controllers
{
    public class TeacherController : Controller
    {
        DataALTeacher dataAccess = new DataALTeacher();

        public IActionResult Index()
        {
            List<EntityTeacher> teacherList = dataAccess.GetAllTeachers();
            return View(teacherList); 
        }
    }
}
