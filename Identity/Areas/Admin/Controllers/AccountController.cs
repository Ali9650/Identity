using Identity.Admin.Models.Account;
using Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Areas.Admin.Controllers
{
	[Area(nameof(Admin))]
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public AccountController(UserManager<User> userManager,
			SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginVM model)
		{
			if (!ModelState.IsValid) return View(model);

			var user = _userManager.FindByEmailAsync(model.EmailAddress).Result;
			if (user is null)
			{
				ModelState.AddModelError(string.Empty, "email ve ya sifre yanlisdir!!");
				return View(model);
			}
			if (!_userManager.IsInRoleAsync(user, "Admin").Result)
			{
				ModelState.AddModelError(string.Empty, "email ve ya sifre yanlisdir!!");
				return View(model);
			}
			var result = _signInManager.PasswordSignInAsync(user, model.Password, false, false).Result;
			if (!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "email veya sifre yanlisdir!!");
				return View(model);
			}

			if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
				return Redirect(model.ReturnUrl);

			return RedirectToAction("index", "dashboard");
		}
	}
}
