using Company.DAL.Models;
using Company.PL.DTOs;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailKitService _mailKitService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailKitService mailKitService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailKitService = mailKitService;
        }

        #region Sign up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDTO model)
        {
            if (ModelState.IsValid) // Server side validation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        //Mapping From SignUpDTO to AppUser
                        user = new AppUser
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                            return RedirectToAction("SignIn");

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }


                }

                ModelState.AddModelError("", "Invalid Sign Up!");


            }
            return View(model);
        }

        #endregion

        #region Sign in

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // Sign In
                        await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid Sign In!");
            }
            return View(model);
        }


        #endregion

        #region Sign out

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        #endregion

        #region Forgot Password

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendResetPasswordUrl(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate Reset Password Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // Create Email
                    var email = new Email
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    // Send Reset Password Email

                    //var flag = EmailSettings.SendEmail(email);

                    var flag = await _mailKitService.SendEmailAsync(email);
                    if (flag)
                    {
                        // Check your Inbox
                        return RedirectToAction("CheckYourInbox");
                    }
                }
                ModelState.AddModelError("", "Invalid Email!");

            }
            return View("ForgotPassword", model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                // Reset Password
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email is null || token is null)
                    return BadRequest();

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction("SignIn");

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }


                ModelState.AddModelError("", "Invalid Reset Password!");

            }
            return View(model);
        }

        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
