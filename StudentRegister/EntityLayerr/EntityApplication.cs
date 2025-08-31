using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class EntityApplication
    {
        private int applicationID;
        private int studentID;
        private int courseID;

        public int ApplicationID { get => applicationID; set => applicationID = value; }
        public int StudentID { get => studentID; set => studentID = value; }
        public int CourseID { get => courseID; set => courseID = value; }
    }
}
