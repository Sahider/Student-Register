using System.Collections.Generic;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class BLLTeacher
    {
        DataALTeacher dalTeacher = new DataALTeacher();

        public List<EntityTeacher> GetTeachers()
        {
            return dalTeacher.GetAllTeachers();
        }

    }
}
