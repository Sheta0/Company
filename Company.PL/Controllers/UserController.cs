using Company.DAL.Models;
using Company.PL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDTO> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(u => new UserToReturnDTO()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Roles = _userManager.GetRolesAsync(u).Result
                });
            }
            else
            {
                users = _userManager.Users.Select(u => new UserToReturnDTO()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Roles = _userManager.GetRolesAsync(u).Result
                }).Where(u => u.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(users);
        }

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id == null)
                return BadRequest("Invalid Id");

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound(new { statusCode = 404, message = $"User with id {id} was not found" });

            var userToReturn = new UserToReturnDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };

            return View(viewName, userToReturn);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDTO model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                    return BadRequest("Invalid Operation");

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return NotFound(new { statusCode = 404, message = $"User with id {id} was not found" });

                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = "User Updated Successfully!";
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
        public async Task<IActionResult> Delete(string id, UserToReturnDTO model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                    return BadRequest("Invalid Operation");

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                    return NotFound(new { statusCode = 404, message = $"User with id {id} was not found" });

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Message"] = "User Deleted Successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Invalid Operation");
            }
            return View(model);
        }
    }
}
