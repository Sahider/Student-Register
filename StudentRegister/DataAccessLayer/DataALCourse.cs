using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using EntityLayer;

namespace DataAccessLayer
{
    public class DataALCourse
    {
        // Listeleme metodu: Derslerin listesini alır
        public static List<EntityCourse> CourseList()
        {
            List<EntityCourse> courseList = new List<EntityCourse>();

            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM TBLCOURSE WHERE IsDeleted = 0", connection))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                EntityCourse course = new EntityCourse
                                {
                                    CourseID = Convert.ToInt32(dr["CourseID"]),
                                    CourseName = dr["CourseName"].ToString(),
                                    MinCapacity = Convert.ToInt32(dr["minCapacity"]),
                                    MaxCapacity = Convert.ToInt32(dr["maxCapacity"])
                                };

                                courseList.Add(course);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata loglamayı unutmayın
                throw new Exception("CourseList hatası: " + ex.Message);
            }

            return courseList;
        }

        // Güncelleme metodu: Bir dersin bilgilerini günceller
        public static int UpdateCourseDAL(EntityCourse course)
        {
            try
            {
                using (SqlConnection connection = Connection.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("UPDATE TBLCOURSE SET CourseName = @name, minCapacity = @min, maxCapacity = @max WHERE CourseID = @id", connection);

                    // Parametreleri daha güvenli bir şekilde ekleyelim
                    cmd.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = course.CourseName;
                    cmd.Parameters.Add("@min", System.Data.SqlDbType.Int).Value = course.MinCapacity;
                    cmd.Parameters.Add("@max", System.Data.SqlDbType.Int).Value = course.MaxCapacity;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = course.CourseID;

                    connection.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Hata loglamayı unutmayın
                throw new Exception("UpdateCourseDAL hatası: " + ex.Message);
            }
        }
    }
}
