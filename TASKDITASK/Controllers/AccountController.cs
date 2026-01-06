using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TASKDITASK.Contexts;
using TASKDITASK.Models;
using TASKDITASK.ViewModels.UserViewModels;

namespace TASKDITASK.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager ) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);


            var existUser = await _userManager.FindByNameAsync(vm.UserName);
            if (existUser is { })
            {
                ModelState.AddModelError("Username", "This username is already exist");
                return View(vm);
            }
            existUser = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if (existUser is { })
            {
                ModelState.AddModelError(nameof(vm.EmailAddress), "This email is already exist");
                return View(vm);
            }



            AppUser newUser = new()
            {
                FullName = vm.FirstName + " " + vm.LastName,
                Email = vm.EmailAddress,
                UserName=vm.UserName
            };

            var result = await _userManager.CreateAsync(newUser, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(vm);
            }


            return Ok("ok");

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.EmailAddress);

            if (user is null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }

            var loginResult = await _userManager.CheckPasswordAsync(user, vm.Password);

            if (!loginResult)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(vm);
            }
            await _signInManager.SignInAsync(user, false);
            return Ok($"{user.FullName} Welcome");
        }
        public async Task<IActionResult> LogOut()
        {
             await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
