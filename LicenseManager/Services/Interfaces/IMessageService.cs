using System.Net.Mail;
using LicenseManager.Models;

namespace LicenseManager.Services.Interfaces
{
    public interface IMessageService
    {
        void SendEmailChangeConfirmationNotice(MailAddress newEmailAddress, string confirmationUrl);
        void SendPasswordResetInstructions(User user, string resetPasswordUrl, bool forgotPassword);
        void SendCredentialRemovedNotice(User user, Credential removed);
        void SendCredentialAddedNotice(User user, Credential added);
    }
}