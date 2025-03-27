using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.DTOs;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController
            (
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;

            if (string.IsNullOrEmpty(SearchInput))
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            else
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
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
        public async Task<IActionResult> Create(EmployeeDTO model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                    model.ImageName = FileSettings.UploadFile(model.Image, "images");

                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);

                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {

                    TempData["Message"] = "Employee Added Successfully!";

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            //var departments = _departmentRepository.GetAll();
            //ViewBag.Departments = departments;
            if (id is null)
                return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null)
                return NotFound(new { statusCode = 404, message = $"Employee with id {id} Not Found" });

            var dto = _mapper.Map<EmployeeDTO>(employee);
            return View(viewName, dto);
        }

        public Task<IActionResult> Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeDTO model)
        {
            if (ModelState.IsValid)
            {

                if (id != model.Id)
                    return BadRequest("Invalid Id");

                if (model.ImageName is not null && model.Image is not null)
                    FileSettings.DeleteFile(model.ImageName, "images");

                if (model.Image is not null)
                    model.ImageName = FileSettings.UploadFile(model.Image, "images");


                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "User Updated Successfully!";
                    return RedirectToAction("Index");
                }
            }


            return View(model);
        }

        public Task<IActionResult> Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeDTO model)
        {
            if (id != model.Id)
                return BadRequest("Invalid Id");

            var employee = _mapper.Map<Employee>(model);
            _unitOfWork.EmployeeRepository.Delete(employee);
            var count = await _unitOfWork.CompleteAsync();

            if (count > 0)
            {
                if (model.ImageName is not null)
                    FileSettings.DeleteFile(model.ImageName, "images");

                TempData["Message"] = "User Deleted Successfully!";

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
