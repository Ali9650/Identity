using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels.Account
{
	public class LoginVM
	{

		[Required(ErrorMessage = "mutleq daxil edilmeli ")]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; }

		[Required(ErrorMessage = "mutleq daxil edilmeli ")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string ? ReturnUrl { get; set; }	
	}
}
