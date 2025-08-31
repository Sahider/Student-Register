using DataAccessLayer;
using EntityLayer;
using System.Collections.Generic;
using System.Linq;  // Linq'i ekledik çünkü FirstOrDefault metodunu kullanacağız

public static class BLLApplication
{
    // Öğrenci ve ders ilişkilerini alır
    public static List<StudentApplicationViewModel> GetApplications()
    {
        return DataALApplication.GetStudentApplications();
    }

    // Öğrenciye ders ekler
    public static int AddApplication(int studentId, int courseId)
    {
        // Öğrencinin veritabanında olup olmadığını kontrol et
        var studentExists = DataALStudent.GetStudentById(studentId); // Burada öğrenci ID'sine göre öğrenci arıyoruz

        if (studentExists == null)
        {
            return 0; // Öğrenci bulunamadı
        }

        // Öğrenci zaten bu derse kayıtlı mı, kontrol et
        var existingApplication = DataALApplication.GetStudentApplications()
            .FirstOrDefault(a => a.StudentID == studentId && a.CourseID == courseId);

        if (existingApplication != null)
        {
            return 0; // Öğrenci zaten bu derse kayıtlı
        }

        // Yeni öğrenci-kurs kaydını ekle
        return DataALApplication.AddStudentCourse(studentId, courseId);
    }

    // Öğrenci ve ders bilgisini günceller
    public static int UpdateApplication(int studentId, int courseId)
    {
        return DataALApplication.UpdateStudentCourse(studentId, courseId); // Veritabanında güncelleme
    }

    // Öğrenci ve ders bilgisini siler
    public static int DeleteApplication(int studentId, int courseId)
    {
        return DataALApplication.DeleteStudentCourse(studentId, courseId);  // Öğrenci ve ders ilişkisini sil
    }

    // Dersleri almak için yeni metot
    public static List<EntityCourse> GetCourses()
    {
        return DataALCourse.CourseList(); // DataALCourse sınıfını çağırıyoruz
    }

    // Öğrencileri getir
    public static List<EntityStudent> GetStudents()
    {
        return DataALStudent.GetStudentList(); // Öğrencileri veritabanından çeker
    }

    // Öğrenciyi ID ile alır
    public static EntityStudent GetStudentById(int studentId)
    {
        return DataALStudent.GetStudentById(studentId); // Öğrenciyi ID ile getir
    }
}
