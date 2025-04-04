using Company.DAL.Models;
using Company.PL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index(string? SearchInput)
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

        public IActionResult Search(string SearchInput)
        {
            var roles = _roleManager.Roles
                .Where(r => r.Name.ToLower().Contains(SearchInput.ToLower()))
                .Select(r => new RoleToReturnDTO
                {
                    Id = r.Id,
                    Name = r.Name
                });
            return PartialView("PartialViews/_RoleTablePartialView", roles);
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

        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound(new { statusCode = 404, message = $"Role with id {roleId} was not found" });

            ViewData["RoleId"] = roleId;

            var usersInRole = new List<UsersInRoleDTO>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                usersInRole.Add(new UsersInRoleDTO()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(usersInRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleDTO> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound(new { statusCode = 404, message = $"Role with id {roleId} was not found" });

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = _userManager.FindByIdAsync(user.UserId).Result;
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                        }

                    }

                }
                TempData["Message"] = "Users Updated Successfully!";
                return RedirectToAction("Edit", new { id = roleId });
            }
            return View(users);
        }

    }
}
