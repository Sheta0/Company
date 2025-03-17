using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchInput))
                employees = _employeeRepository.GetAll();
            else
                employees = _employeeRepository.GetByName(SearchInput);
            //// Dictionary : 3 Properties
            //// 1.ViewData : Transfer Data from Controller (Action) to View
            //ViewData["Message"] = "Hello From ViewData!";

            //// 2.ViewBag  : Transfer Data from Controller (Action) to View
            //ViewBag.Message = new { Message = "Hello From ViewBag!" };

            // 3.TempData 

            return View(employees);
        }

        public IActionResult Create()
        {
            //var departments = _departmentRepository.GetAll();
            //ViewBag.Departments = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeDTO model)
        {
            if (ModelState.IsValid)
            {
                // Manual Mapping
                //var employee = new Employee
                //{
                //    Name = model.Name,
                //    Age = model.Age,
                //    Email = model.Email,
                //    Address = model.Address,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    HiringDate = model.HiringDate,
                //    DepartmentId = model.DepartmentId
                //};

                var employee = _mapper.Map<Employee>(model);
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee Added Successfully!";
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            //var departments = _departmentRepository.GetAll();
            //ViewBag.Departments = departments;
            if (id is null)
                return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound(new { statusCode = 404, message = $"Employee with id {id} Not Found" });

            var dto = _mapper.Map<EmployeeDTO>(employee);
            return View(viewName, dto);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeDTO model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                    return BadRequest("Invalid Id");

                var employee = _mapper.Map<Employee>(model);
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                    return RedirectToAction("Index");
            }


            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest("Invalid Id");

            var count = _employeeRepository.Delete(employee);
            if (count > 0)
                return RedirectToAction("Index");
            return View(employee);
        }
    }
}
