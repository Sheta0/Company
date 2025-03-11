﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Models
{
    public class Employee : BaseEntity 
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? Age { get; set; }
        
        public string Email { get; set; }

        public string Address { get; set; }
        
        public string Phone { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
        
        public bool IsDeleted { get; set; }

        public DateTime HiringDate { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
