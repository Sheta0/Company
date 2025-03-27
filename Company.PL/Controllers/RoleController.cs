using Company.DAL.Models;
using Company.PL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDTO> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(r => new RoleToReturnDTO()
                {
                    Id = r.Id,
                    Name = r.Name
                });
            }
            else
            {
                roles = _roleManager.Roles.Select(r => new RoleToReturnDTO()
                {
                    Id = r.Id,
                    Name = r.Name
                }).Where(r => r.Name.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleToReturnDTO model)
        {
            if (ModelState.IsValid)
            {
                var roleResult = await _roleManager.FindByNameAsync(model.Name);
                if (roleResult is not null)
                    ModelState.AddModelError("", "Role already exists");

                var role = new IdentityRole()
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Role Created Successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Invalid Operation");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id == null)
                return BadRequest("Invalid Id");

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound(new { statusCode = 404, message = $"Role with id {id} was not found" });

            var roleToReturn = new RoleToReturnDTO()
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(viewName, roleToReturn);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDTO model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                    return BadRequest("Invalid Operation");

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return NotFound(new { statusCode = 404, message = $"Role with id {id} was not found" });

                var roleResult = await _roleManager.FindByNameAsync(model.Name);
                if (roleResult is not null)
                    ModelState.AddModelError("", "Role already exists");


                role.Name = model.Name;

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Role Updated Successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Invalid Operation");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, RoleToReturnDTO model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                    return BadRequest("Invalid Operation");

                var role = await _roleManager.FindByIdAsync(id);

                if (role is null)
                    return NotFound(new { statusCode = 404, message = $"Role with id {id} was not found" });

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Role Deleted Successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Invalid Operation");
            }
            return View(model);
        }
    }
}
