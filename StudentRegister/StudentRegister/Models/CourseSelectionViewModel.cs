using EntityLayer;
using System.Collections.Generic;

namespace StudentRegister.Models
{
    public class CourseSelectionViewModel
    {
        public List<EntityCourse> Courses { get; set; } = new();
        public int SelectedCourseID { get; set; }
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }
    }


}
