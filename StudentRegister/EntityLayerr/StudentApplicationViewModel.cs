using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class StudentApplicationViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string StudentNumber { get; set; }
        public string StudentMail { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
    }
}



