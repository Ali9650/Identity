using Identity.Entities;
using Identity.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = new User
            {
				UserName = model.EmailAddress,
				Email = model.EmailAddress,
                Country=model.Country,
                City=model.City,
                PhoneNumber=model.PhoneNumber,
            };
            var result = _userManager.CreateAsync(user, model.Password).Result;

			if (!result.Succeeded)
            {
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
                 return View(model);
            }
            return RedirectToAction(nameof(Login));
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

			var user = _userManager.FindByEmailAsync (model.EmailAddress).Result;
			if (user is null)
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
            return RedirectToAction("index","home");
        }

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}

	}
    
}
