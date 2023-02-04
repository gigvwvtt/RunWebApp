using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RunWebApp.ViewModels
{
	public class RegisterViewModel
	{
		[Display(Name = "Email Address")]
		[Required(ErrorMessage = "Email address is required")]
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name = "Confirm password")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Confirm password is required")]
		[Compare("Password", ErrorMessage = "Password do not match")]
		public string ConfirmPassword { get; set; }
	}
}
