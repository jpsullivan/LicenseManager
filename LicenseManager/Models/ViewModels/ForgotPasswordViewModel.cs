using System.ComponentModel.DataAnnotations;

namespace LicenseManager.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}