using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TASKDITASK.Abstraction;
using TASKDITASK.Contexts;
using TASKDITASK.Models;
using TASKDITASK.ViewModels.UserViewModels;

namespace TASKDITASK.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager, IEmailService _emailService) : Controller
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
                UserName = vm.UserName
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
            await SendConfirmationMailAsync(newUser);
            TempData["SuccessMessage"] = "Registerdan ugurla kecdiniz zehmet olmasa emailinizi tesdiqleyin";

            return RedirectToAction("Login");

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

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email");
                await SendConfirmationMailAsync(user);
                return View(vm);
            }

            await _signInManager.SignInAsync(user, vm.IsRemember);

            return RedirectToAction(nameof(Index), "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "User"
            });

            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Admin"
            });

            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Moderator"
            });

            return Ok("Roles created");
        }
        private async Task SendConfirmationMailAsync(AppUser user)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action("ConfirmEmail", "Account", new { token, userId = user.Id }, Request.Scheme);
            string emailBody = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>Confirm your email</title>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">

    <style>
        body {{
            margin: 0;
            padding: 0;
            background-color: #f4f6f8;
            font-family: Arial, Helvetica, sans-serif;
        }}
        table {{
            border-collapse: collapse;
        }}
        a {{
            text-decoration: none;
        }}
    </style>
</head>
<body>

<table width=""100%"" cellpadding=""0"" cellspacing=""0"" bgcolor=""#f4f6f8"">
    <tr>
        <td align=""center"" style=""padding:40px 10px;"">

            <table width=""600"" cellpadding=""0"" cellspacing=""0"" bgcolor=""#ffffff"" style=""border-radius:8px; overflow:hidden;"">

                <!-- Header -->
                <tr>
                    <td style=""background:#2563eb; padding:20px; text-align:center; color:#ffffff;"">
                        <h2 style=""margin:0;"">Confirm Your Email</h2>
                    </td>
                </tr>

                <!-- Body -->
                <tr>
                    <td style=""padding:30px; color:#333333; line-height:1.6;"">
                        <p>Hello <strong>{user.FullName}</strong>,</p>

                        <p>
                            Thank you for creating an account. Please confirm your email address by clicking the button below.
                        </p>

                        <table cellpadding=""0"" cellspacing=""0"" style=""margin:30px auto;"">
                            <tr>
                                <td align=""center"">
                                    <a href=""{url}""
                                       style=""background:#2563eb; color:#ffffff; padding:14px 28px;
                                              border-radius:6px; font-weight:bold; display:inline-block;"">
                                        Confirm Email
                                    </a>
                                </td>
                            </tr>
                        </table>

                        <p>
                            If the button doesn’t work, copy and paste this link into your browser:
                        </p>

                        <p style=""word-break:break-all;"">
                            <a href=""{url}"" style=""color:#2563eb;"">
                                {url}
                            </a>
                        </p>

                        <p>
                            If you didn’t create this account, you can safely ignore this email.
                        </p>

                        <p>
                            Regards,<br>
                            <strong>Your Application Team</strong>
                        </p>
                    </td>
                </tr>

                <!-- Footer -->
                <tr>
                    <td style=""background:#f1f5f9; padding:15px; text-align:center; font-size:12px; color:#666666;"">
                        © 2026 Pronia
                    </td>
                </tr>

            </table>

        </td>
    </tr>
</table>

</body>
</html>
";
            await _emailService.SendEmailAsync(user.Email!, "Confirm your email", emailBody);
        }

        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }


    }
}
