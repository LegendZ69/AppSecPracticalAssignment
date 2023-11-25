using System.ComponentModel.DataAnnotations;

namespace AppSecPracticalAssignment.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Email Address is required")]
        [MaxLength(254, ErrorMessage = "Email must be at most 254 characters")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^(?=.{1,254}$)(?=.{1,63}@)(?![.])[\w!#$%&'*+\-/=?^_`{|}~]+(\.[\w!#$%&'*+\-/=?^_`{|}~]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$", ErrorMessage = "Email must be between 1 and 254 characters and does not contain dangerous characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(11, ErrorMessage = "Password must be at least 12 characters")]
        [MaxLength(64, ErrorMessage = "Password must be at most 64 characters")]
        [DataType(DataType.Password)]
        /*        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!#$%&'*+\-/=?^_{|}~]+)*[a-zA-Z\d!#$%&'*+\-/=?^_{|}~]{12,64}$", ErrorMessage = "Password must be between 12 and 64 characters and contain at least an uppercase, lowercase, digit and a symbol")]
        */
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
