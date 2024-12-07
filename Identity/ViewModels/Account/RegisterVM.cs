using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels.Account
{
    public class RegisterVM
    {
            [Required(ErrorMessage = "mutleq daxil edilmeli ")]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]
            public string EmailAddress { get; set; }

            [Required(ErrorMessage = "mutleq daxil edilmeli ")]
            public string Country { get; set; }

            [Required(ErrorMessage = "mutleq daxil edilmeli ")]
            public string City { get; set; }

            public string? PhoneNumber { get; set; }

            [Required(ErrorMessage = "mutleq daxil edilmeli ")]
            [DataType(DataType.Password)]
            public string Password { get; set; }


            [Required(ErrorMessage = "mutleq daxil edilmeli ")]
            [Compare(nameof(Password), ErrorMessage = "sifreler eyni deyil")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }
    }


