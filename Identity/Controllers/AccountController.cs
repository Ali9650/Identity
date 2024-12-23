﻿using Identity.Entities;
using Identity.Utilities.EmailHandler.Abstract;
using Identity.Utilities.EmailHandler.Models;
using Identity.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IEmailService _emailService;

		public AccountController(UserManager<User> userManager,
			SignInManager<User> signInManager,
			RoleManager<IdentityRole> roleManager,
			IEmailService emailService
			)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_emailService = emailService;
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
				Email = model.EmailAddress,
				UserName = model.EmailAddress,
				PhoneNumber = model.PhoneNumber,
				Country = model.Country,
				City = model.City,
			};
			var result = _userManager.CreateAsync(user, model.Password).Result;
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

				return View(model);
			}
			var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
			var url = Url.Action(nameof(ConfirmEmail), "Account", new { token, user.Email }, Request.Scheme);
			_emailService.SendMessage(new Message(new List<string> { user.Email }, "email confirmation", url));
			return RedirectToAction(nameof(Login));
		}

		

		public IActionResult ConfirmEmail(string email, string token)
		{
			var user = _userManager.FindByEmailAsync(email).Result;
			if (user is null) return NotFound();

			var result = _userManager.ConfirmEmailAsync(user, token).Result;
			if (!result.Succeeded) return NotFound();

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
			if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
				return Redirect(model.ReturnUrl);
			return RedirectToAction("index","home");
        }

		[HttpGet]
		public IActionResult ResetPassword()
		{
			return View();
		}
		[HttpPost]
		public IActionResult ResetPassword(ResetPasswordVM model)
		{
			if (!ModelState.IsValid) return View(model);
			var user = _userManager.FindByEmailAsync(model.Email).Result;
			if (user is null)
			{
				ModelState.AddModelError("NewPassword", "Cannot reset password");
				return View(model);
			}
			var result = _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword).Result;
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

				return View(model);
			}
			return RedirectToAction(nameof(Login));
		}

		[HttpGet]
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ForgetPassword(ForgotPasswordVM model)
		{
			if (!ModelState.IsValid) return View(model);
			var user = _userManager.FindByEmailAsync(model.Email).Result;
			if (user is null)
			{
				ModelState.AddModelError("Email", "user not found");
				return View(model);
			}
			var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
			var url = Url.Action(nameof(ResetPassword), "Account", new { token, user.Email }, Request.Scheme);
			_emailService.SendMessage(new Message(new List<string> { user.Email }, "identity forgot password", url));
			ViewBag.Not = "mail sended for reset password";
			return View("Notification");
		}


		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}

	}
    
}
