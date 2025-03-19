using Company.BLL.Repositories;
using Company.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Company.PL.DTOs;
using Company.DAL.Models;
using AutoMapper;

namespace Company.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        // Ask CLR to create an object of DepartmentRepository
        public DepartmentController
            (
            //IDepartmentRepository departmentRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet] // GET: Department/Index
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet] // GET: Department/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepository.Add(department);
                var count = _unitOfWork.Complete();

                if (count > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest("Invalid Id"); // 400

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);

            if (department is null)
                return NotFound(new {statusCode = 404, message = $"Department with Id {id} is not found"});

            var dto = _mapper.Map<DepartmentDTO>(department);

            return View(viewName, dto);
        }

        public IActionResult Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest("Invalid Id"); // 400

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null)
            //    return NotFound(new { statusCode = 404, message = $"Department with Id {id} is not found" });

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, DepartmentDTO model)
        {
            if (ModelState.IsValid)
            {
                if(id != model.Id) return BadRequest(); // 400
                var department = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepository.Update(department);
                var count = _unitOfWork.Complete();

                if (count > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete (int id)
        {
            //var department = _departmentRepository.Get(id);
            //if (department is null)
            //    return NotFound(new { statusCode = 404, message = $"Department with Id {id} is not found" });
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, DepartmentDTO model)
        {
            if (ModelState.IsValid) 
            {
                if (id != model.Id) return BadRequest(); // 400
                var department = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepository.Delete(department);
                var count = _unitOfWork.Complete();

                if (count > 0)
                    return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
