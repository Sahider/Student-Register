using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class EntityStudent
    {
       

        [Required(ErrorMessage = "Identity number is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Identity number must be exactly 11 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Identity number must be numeric.")]
        public string Kimlik { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("^[a-zA-ZğüşıöçĞÜŞİÖÇ\\s]+$", ErrorMessage = "Only letters are allowed.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [RegularExpression("^[a-zA-ZğüşıöçĞÜŞİÖÇ\\s]+$", ErrorMessage = "Only letters are allowed.")]
        public string Surname { get; set; }

        public int StudentID { get; set; }

        [Required(ErrorMessage = "Number is required.")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Number must be exactly 5 characters.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Number must be numeric.")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "Password must be at least 6 characters long and include an uppercase letter, a lowercase letter, a number, and a special character.")]
        public string Password { get; set; }
      
    }
}

