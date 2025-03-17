using AutoMapper;
using Company.DAL.Models;
using Company.PL.DTOs;

namespace Company.PL.Mapping
{
    // CLR
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.EmpName));
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(d => d.EmpName, o => o.MapFrom(s => s.Name));
        }
    }
}
