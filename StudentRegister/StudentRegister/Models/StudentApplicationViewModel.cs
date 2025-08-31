using System.ComponentModel.DataAnnotations;

namespace StudentRegister.Models

{
    public class StudentApplicationViewModel
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "İsim zorunludur.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "İsim sadece harf içermelidir.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Soyisim zorunludur.")]
        [RegularExpression(@"^[a-zA-ZğüşöçİĞÜŞÖÇ\s]+$", ErrorMessage = "Soyisim sadece harf içermelidir.")]
        public string StudentSurname { get; set; }

        [Required(ErrorMessage = "Öğrenci numarası zorunludur.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Öğrenci numarası sadece rakamlardan oluşmalıdır.")]
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string StudentMail { get; set; }

        [Required(ErrorMessage = "Ders adı zorunludur.")]
        public string CourseName { get; set; }
    }
}
