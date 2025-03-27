using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email is Required!")]
        [EmailAddress(ErrorMessage = "Email is not valid!")]
        public string Email { get; set; }
    }
}
