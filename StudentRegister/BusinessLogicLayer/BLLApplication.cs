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
        var studentExists = DataALStudent.GetStudentById(studentId);

        if (studentExists == null)
        {
            return 0; // Öğrenci bulunamadı
        }

        var existingApplication = DataALApplication.GetStudentApplications()
            .FirstOrDefault(a => a.StudentID == studentId && a.CourseID == courseId);

        if (existingApplication != null)
        {
            return 0; // Öğrenci zaten bu derse kayıtlı
        }

        return DataALApplication.AddStudentCourse(studentId, courseId);
    }

    // Öğrenci ve ders bilgisini günceller
    public static int UpdateApplication(int studentId, int courseId)
    {
        return DataALApplication.UpdateStudentCourse(studentId, courseId);
    }

    // Öğrenci ve ders bilgisini siler
    public static int DeleteApplication(int studentId, int courseId)
    {
        return DataALApplication.DeleteStudentCourse(studentId, courseId);
    }

    // Dersleri almak için yeni metot
    public static List<EntityCourse> GetCourses()
    {
        return DataALCourse.CourseList();
    }

    // Öğrencileri getir
    public static List<EntityStudent> GetStudents()
    {
        return DataALStudent.GetStudentList();
    }

    // Öğrenciyi ID ile alır
    public static EntityStudent GetStudentById(int studentId)
    {
        return DataALStudent.GetStudentById(studentId);
    }

    // Öğrencinin mevcut derslerini alır
    public static List<StudentApplicationViewModel> GetStudentCoursesByStudentId(int studentId)
    {
        return DataALApplication.GetStudentApplications()
            .Where(a => a.StudentID == studentId)
            .ToList();
    }
}
