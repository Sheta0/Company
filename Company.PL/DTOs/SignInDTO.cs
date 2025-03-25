using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class SignInDTO
    {
        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Email is not valid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
