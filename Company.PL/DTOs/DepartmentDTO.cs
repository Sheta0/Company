using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class DepartmentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is Required!")]
        public DateTime CreateAt { get; set; }
    }
}
