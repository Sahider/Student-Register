using DataAccessLayer;
using EntityLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

public static class DataALApplication
{
    // Öğrenci ve ders bilgilerini alır
    public static List<StudentApplicationViewModel> GetStudentApplications()
    {
        List<StudentApplicationViewModel> list = new List<StudentApplicationViewModel>();

        try
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(@"
                    SELECT A.StudentID, S.StudentName, S.StudentSurname, S.StudentNumber, S.StudentMail, C.CourseName, C.CourseID
                    FROM TBLAPPLICATION A
                    INNER JOIN TBLSTUDENT S ON A.StudentID = S.StudentID
                    INNER JOIN TBLCOURSE C ON A.CourseID = C.CourseID", connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new StudentApplicationViewModel
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        StudentName = reader["StudentName"].ToString(),
                        StudentSurname = reader["StudentSurname"].ToString(),
                        StudentNumber = reader["StudentNumber"].ToString(),
                        StudentMail = reader["StudentMail"].ToString(),
                        CourseName = reader["CourseName"].ToString(),
                         CourseID = Convert.ToInt32(reader["CourseID"])

                    });
                }

                reader.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("GetStudentApplications hatası: " + ex.Message);
        }

        return list;
    }

    // Öğrenciye yeni ders ekler
    public static int AddStudentCourse(int studentId, int courseId)
    {
        using (SqlConnection connection = Connection.GetConnection())
        {
            connection.Open();

            // Aynı öğrenci ve ders için kayıt kontrolü
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM TBLAPPLICATION WHERE StudentID=@studentId AND CourseID=@courseId", connection);
            checkCmd.Parameters.AddWithValue("@studentId", studentId);
            checkCmd.Parameters.AddWithValue("@courseId", courseId);

            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                // Kayıt zaten var
                return 0;
            }

            // Yeni kayıt ekle
            SqlCommand insertCmd = new SqlCommand("INSERT INTO TBLAPPLICATION (StudentID, CourseID) VALUES (@studentId, @courseId)", connection);
            insertCmd.Parameters.AddWithValue("@studentId", studentId);
            insertCmd.Parameters.AddWithValue("@courseId", courseId);

            return insertCmd.ExecuteNonQuery(); // Başarıyla eklenirse, 1 döner
        }
    }

    // Öğrenci ve ders bilgisini günceller
    public static int UpdateStudentCourse(int studentId, int courseId)
    {
        using (SqlConnection connection = Connection.GetConnection())
        {
            // Öğrenci ve ders ilişkisinin zaten var olup olmadığını kontrol et
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM TBLAPPLICATION WHERE StudentID = @studentId AND CourseID = @courseId", connection);
            checkCmd.Parameters.AddWithValue("@studentId", studentId);
            checkCmd.Parameters.AddWithValue("@courseId", courseId);
            connection.Open();

            int count = (int)checkCmd.ExecuteScalar();

            if (count > 0)
            {
                // Eğer öğrenci zaten bu derse kayıtlıysa, hiçbir şey yapma
                return 0;
            }

            // Öğrenciye yeni ders ekle
            SqlCommand insertCmd = new SqlCommand("INSERT INTO TBLAPPLICATION (StudentID, CourseID) VALUES (@studentId, @courseId)", connection);
            insertCmd.Parameters.AddWithValue("@studentId", studentId);
            insertCmd.Parameters.AddWithValue("@courseId", courseId);

            return insertCmd.ExecuteNonQuery();
        }
    }


    // Öğrenci ve ders bilgisini siler
    public static int DeleteStudentCourse(int studentId, int courseId)
    {
        using (SqlConnection connection = Connection.GetConnection())
        {
            connection.Open();

            // Öğrenci ve ders ilişkisini silme
            SqlCommand cmd = new SqlCommand("DELETE FROM TBLAPPLICATION WHERE StudentID = @studentId AND CourseID = @courseId", connection);
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@courseId", courseId);

            return cmd.ExecuteNonQuery(); // Başarıyla silindiyse 1 dönecektir
        }
    }

}