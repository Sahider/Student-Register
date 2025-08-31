using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DataALTeacher
    {
        private readonly string connectionString = "Server=localhost;Database=SchoolReg;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

        public List<EntityTeacher> GetAllTeachers()
        {
            List<EntityTeacher> teacherList = new List<EntityTeacher>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TBLTEACHER", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    EntityTeacher teacher = new EntityTeacher
                    {
                        TeacherID = (int)reader["TeacherID"],
                        TeacherName = reader["TeacherName"].ToString(),
                        TeacherSurname = reader["TeacherSurname"].ToString(),
                        TeacherEmail = reader["TeacherEmail"].ToString(),
                        TeacherSubject = reader["TeacherSubject"].ToString()
                    };
                    teacherList.Add(teacher);
                }
            }
            return teacherList;
        }

    }
}
