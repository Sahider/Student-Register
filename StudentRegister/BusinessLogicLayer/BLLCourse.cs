using DataAccessLayer;
using EntityLayer;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer
{
    public class BLLCourse
    {
        public static List<EntityCourse> GetCourseList()
        {
            return DataALCourse.CourseList();
        }
        public static int UpdateCourseBLL(EntityCourse course)
        {
            return DataALCourse.UpdateCourseDAL(course);
        }

        public static int AddCourse(EntityCourse course)
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand
                    ("INSERT INTO TBLCOURSE (CourseName, minCapacity, maxCapacity) " +
                    "VALUES (@name, @min, @max)", connection);
                cmd.Parameters.AddWithValue("@name", course.CourseName);
                cmd.Parameters.AddWithValue("@min", course.MinCapacity);
                cmd.Parameters.AddWithValue("@max", course.MaxCapacity);

                connection.Open();
                return cmd.ExecuteNonQuery(); // 1 veya 0 döndürür
            }
        }


        public static int UpdateCourseDAL(EntityCourse course)
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand
                    ("UPDATE TBLCOURSE SET CourseName = @name, minCapacity = @min, maxCapacity = @max WHERE CourseID = @id", connection);
                cmd.Parameters.AddWithValue("@name", course.CourseName);
                cmd.Parameters.AddWithValue("@min", course.MinCapacity);
                cmd.Parameters.AddWithValue("@max", course.MaxCapacity);
                cmd.Parameters.AddWithValue("@id", course.CourseID);

                connection.Open();
                return cmd.ExecuteNonQuery();
            }
        }


        public static int DeleteCourse(int courseId)
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                connection.Open();

                SqlCommand cmd1 = new SqlCommand("UPDATE TBLTEACHER SET TeacherSubject = NULL WHERE TeacherSubject = @CourseID", connection);
                cmd1.Parameters.AddWithValue("@CourseID", courseId);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("DELETE FROM TBLCOURSE WHERE CourseID = @CourseID", connection);
                cmd2.Parameters.AddWithValue("@CourseID", courseId);
                return cmd2.ExecuteNonQuery();
            }
        }


        public static EntityCourse GetCourseById(int id)
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TBLCOURSE WHERE IsDeleted = 0 AND CourseID = @id", connection);
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new EntityCourse
                        {
                            CourseID = Convert.ToInt32(dr["CourseID"]),
                            CourseName = dr["CourseName"].ToString(),
                            MinCapacity = Convert.ToInt32(dr["minCapacity"]),
                            MaxCapacity = Convert.ToInt32(dr["maxCapacity"])
                        };
                    }
                }
            }

            return null;
        }
    }
}



































/* Data Access Layer (DAL - DataALCourse)

Veritabanıyla doğrudan iletişim kurduğumuz katman.

SQL sorgularını çalıştırıyor, verileri çekiyor, ekliyor, güncelliyor, siliyor.

Bağlantıyı açıp kapatıyor, SqlCommand ve SqlDataReader kullanıyor.

Business Logic Layer (BLL - BLLCourse)

DAL’dan gelen veriyi işleyip, doğrulamalar yapıyor.

Örneğin; ders adı boş mu, kapasite değerleri mantıklı mı diye kontrol ediyor.

Bu kontrollerden geçerse DAL’daki metotları çağırıyor.

Böylece uygulamanın kuralları burada uygulanıyor.

Özetle:
DAL sadece veri işlemlerini yapar, BLL ise o veriye kural bazlı mantık ekler ve DAL metodlarını kullanır. Bu sayede kod daha temiz, yönetilebilir ve güvenli olur. */ 