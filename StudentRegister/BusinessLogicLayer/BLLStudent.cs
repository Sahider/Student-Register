using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class BLLStudent
    {
        public static int AddStudentBLL(EntityStudent prmt)
        {
            if (!string.IsNullOrEmpty(prmt.Kimlik) &&
                !string.IsNullOrEmpty(prmt.Name) &&
                !string.IsNullOrEmpty(prmt.Surname) &&
                !string.IsNullOrEmpty(prmt.Number) &&
                !string.IsNullOrEmpty(prmt.Email) &&
                !string.IsNullOrEmpty(prmt.Password))
            {
                return DataALStudent.AddStudent(prmt);
            }

            return -1;
        }
        public static List<EntityStudent> GetStudentListBLL()
        {
            return DataALStudent.GetStudentList();
        }

        // Kimlik kontrol metodu
        public static bool StudentExistsByKimlik(string kimlik)
        {
            return DataALStudent.GetStudentList().Any(x => x.Kimlik == kimlik);
        }
        // Güncelleme işlemi
        public static bool UpdateStudentBLL(EntityStudent student)
        {
            // Basit validasyon örneği
            if (student.StudentID > 0 &&
                !string.IsNullOrEmpty(student.Name) &&
                !string.IsNullOrEmpty(student.Surname) &&
                !string.IsNullOrEmpty(student.Number) &&
                !string.IsNullOrEmpty(student.Email))
            {
                return DataALStudent.UpdateStudent(student);
            }
            return false;
        }

        // Silme işlemi
        public static bool DeleteStudentBLL(int id)
        {
            if (id > 0)
            {
                return DataALStudent.DeleteStudent(id);
            }
            return false;
        }

        // ID'ye göre öğrenci getirme
        public static EntityStudent GetStudentByIdBLL(int id)
        {
            if (id > 0)
            {
                return DataALStudent.GetStudentById(id);
            }
            return null;
        }
        }
}
