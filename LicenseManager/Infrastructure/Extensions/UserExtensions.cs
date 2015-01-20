using System.Net.Mail;
using LicenseManager.Models;

namespace LicenseManager.Infrastructure.Extensions
{
    public static class UserExtensions
    {
        public static MailAddress ToMailAddress(this User user)
        {
            return new MailAddress(user.EmailAddress, user.Username);
        }
    }
}