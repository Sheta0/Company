using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.DTOs
{
    public class EmployeeDTO
    {
        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int? Age { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid!")]
        public string Email { get; set; }

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z\s]{4,10}-[a-zA-Z\s]{5,10}$", ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Phone is not valid!")]
        public string Phone { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }

        [DisplayName("Date Of Creation")]
        public DateTime CreateAt { get; set; }
        public int? DepartmentId { get; set; }
    }
}
