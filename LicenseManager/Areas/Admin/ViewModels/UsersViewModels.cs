using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LicenseManager.Models;

namespace LicenseManager.Areas.Admin.ViewModels
{
    public class UsersViewModels
    {
        public UserListModel UserList { get; set; }
        public NewUserModel NewUser { get; set; }
    }

    public class UserListModel
    {
        public IEnumerable<User> Users { get; set; }

        public UserListModel(IEnumerable<User> users)
        {
            Users = users;
        }
    }

    public class NewUserModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirmation { get; set; }
    }
}