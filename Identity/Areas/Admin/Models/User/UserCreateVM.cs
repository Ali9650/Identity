using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Identity.Areas.Admin.Models.User
{
	public class UserCreateVM
	{
		public UserCreateVM()
		{
			RolesIds = new List<string>();
		}
		[Required(ErrorMessage = "zehmet olmasa daxil edin  email  ")]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; }

		[Required(ErrorMessage = "zehmet olmasa daxil edin  olke")]
		public string Country { get; set; }

		[Required(ErrorMessage = "zehmet olmasa daxil edin sheher")]
		public string City { get; set; }

		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "zehmet olmasa daxil edin paswword")]
		[DataType(DataType.Password)]
		public string Password { get; set; }


		[Required(ErrorMessage = "zehmet olmasa daxil edin  confirmPaswword")]
		[Compare(nameof(Password), ErrorMessage = "passwordlar eyni deyil")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public List<SelectListItem>? Roles { get; set; }
		public List<string>? RolesIds { get; set; }

	}
}
