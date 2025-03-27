using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Password is Required!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is Required!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and ConfirmPassword do not match!")]
        public string ConfirmPassword { get; set; }

    }
}
