using System.ComponentModel.DataAnnotations;

namespace LicenseManager.Models.ViewModels
{
    public class NewProductVersionViewModel
    {
        [Display(Name = "Version Name")]
        public string Version { get; set; }
    }
}