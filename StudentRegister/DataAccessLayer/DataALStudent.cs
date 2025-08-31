using Microsoft.Data.SqlClient;
using System.Data;
using EntityLayer;

namespace DataAccessLayer
{
    public class DataALStudent
    {
        // Öğrenci ekler
        public static int AddStudent(EntityStudent prmt)
        {
            using (SqlConnection conn = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO TBLSTUDENT (StudentTC, StudentName, StudentSurname, StudentNumber, StudentMail, StudentPassword) VALUES (@p1, @p2, @p3, @p4, @p5, @p6)", conn);
                cmd.Parameters.AddWithValue("@p1", prmt.Kimlik);
                cmd.Parameters.AddWithValue("@p2", prmt.Name);
                cmd.Parameters.AddWithValue("@p3", prmt.Surname);
                cmd.Parameters.AddWithValue("@p4", prmt.Number);
                cmd.Parameters.AddWithValue("@p5", prmt.Email);
                cmd.Parameters.AddWithValue("@p6", prmt.Password);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // Öğrenci siler
        public static bool DeleteStudent(int id)
        {
            using (SqlConnection conn = Connection.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                // Önce bağlı kayıtları sil
                SqlCommand cmd1 = new SqlCommand("DELETE FROM TBLAPPLICATION WHERE StudentID = @id", conn);
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.ExecuteNonQuery();

                // Sonra öğrenciyi sil
                SqlCommand cmd2 = new SqlCommand("DELETE FROM TBLSTUDENT WHERE StudentID = @id", conn);
                cmd2.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd2.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        // Öğrenci günceller
        public static bool UpdateStudent(EntityStudent parametre)
        {
            using (SqlConnection conn = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE TBLSTUDENT SET StudentTC=@p1, StudentName=@p2, StudentSurname=@p3, StudentNumber=@p4, StudentMail=@p5, StudentPassword=@p6 WHERE StudentID=@p7", conn);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd.Parameters.AddWithValue("@p1", parametre.Kimlik);
                cmd.Parameters.AddWithValue("@p2", parametre.Name);
                cmd.Parameters.AddWithValue("@p3", parametre.Surname);
                cmd.Parameters.AddWithValue("@p4", parametre.Number);
                cmd.Parameters.AddWithValue("@p5", parametre.Email);
                cmd.Parameters.AddWithValue("@p6", parametre.Password);
                cmd.Parameters.AddWithValue("@p7", parametre.StudentID);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        // Öğrenci ID'sine göre öğrenci getir
        public static EntityStudent GetStudentById(int id)
        {
            EntityStudent student = null;

            using (SqlConnection conn = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TBLSTUDENT WHERE StudentID = @id", conn);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    student = new EntityStudent
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        Kimlik = reader["StudentTC"].ToString(),
                        Name = reader["StudentName"].ToString(),
                        Surname = reader["StudentSurname"].ToString(),
                        Number = reader["StudentNumber"].ToString(),
                        Email = reader["StudentMail"].ToString(),
                        Password = reader["StudentPassword"].ToString()
                    };
                }

                reader.Close();
            }

            return student;
        }

        // Öğrenci listesini getir
        public static List<EntityStudent> GetStudentList()
        {
            List<EntityStudent> students = new List<EntityStudent>();

            using (SqlConnection conn = Connection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TBLSTUDENT", conn);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EntityStudent student = new EntityStudent
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        Kimlik = reader["StudentTC"].ToString(),
                        Name = reader["StudentName"].ToString(),
                        Surname = reader["StudentSurname"].ToString(),
                        Number = reader["StudentNumber"].ToString(),
                        Email = reader["StudentMail"].ToString(),
                        Password = reader["StudentPassword"].ToString()
                    };
                    students.Add(student);
                }
                reader.Close();
            }

            return students;
        }
    }
}
