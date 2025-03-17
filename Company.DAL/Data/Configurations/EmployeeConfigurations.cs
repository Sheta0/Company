﻿using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Configurations
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> E)
        {
            E.Property(e => e.Salary).HasColumnType("decimal(18,2)");

            E.HasOne(e => e.Department)
             .WithMany(d => d.Employees)
             .HasForeignKey(e => e.DepartmentId)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
