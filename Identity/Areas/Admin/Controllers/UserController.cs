using Identity.Areas.Admin.Models.User;
using Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(UserManager<User> userManager,
			RoleManager<IdentityRole> roleManager
			)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		[HttpGet]
		public IActionResult Index()
		{
			//ViewBag.isUserController = true;
			//ViewBag.isIndexAction = true;
			var users = new List<UserVM>();
			foreach (var user in _userManager.Users.ToList())
			{
				if (!_userManager.IsInRoleAsync(user, "Admin").Result)
				{
					users.Add(new UserVM
					{
						Id = user.Id,
						Email = user.Email,
						Country = user.Country,
						City = user.City,
						PhoneNumber = user.PhoneNumber,
						Roles = _userManager.GetRolesAsync(user).Result.ToList()
					});
				}

			}
			var model = new UserIndexVM
			{
				Users = users
			};
			return View(model);
		}

		[HttpPost]
		public IActionResult Delete(string id)
		{
			var user = _userManager.FindByIdAsync(id).Result;
			if (user == null) return NotFound("istifadeci tapilmadi");
			var deleteResult = _userManager.DeleteAsync(user).Result;
			if (!deleteResult.Succeeded) return NotFound();
			return RedirectToAction(nameof(Index));

		}
	}
}
