using System.ComponentModel.DataAnnotations;
namespace CodingAddaSurfaceController.Models
{
	public class ContactUs
	{
		[Required]
		public string Name { get; set; }	
		[Required]
		public string Email { get; set; }
		[Required]
		public string Message { get; set; }

	}
}
