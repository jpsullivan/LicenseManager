using System;
using System.Collections.Generic;
using System.Linq;
using Jil;
using LicenseManager.Authentication;

namespace LicenseManager.Models
{
    public sealed class User
    {
        public User() { }

        public User(string username)
        {
            Credentials = new List<Credential>();
            Roles = new List<Role>();
            Username = username;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }

        [JilDirective(true)]
        public ICollection<EmailMessage> Messages { get; set; }

        [JilDirective(true)]
        public ICollection<Role> Roles { get; set; }

        [JilDirective(true)]
        public bool EmailAllowed { get; set; }

        [JilDirective(true)]
        public string PasswordResetToken { get; set; }

        [JilDirective(true)]
        public DateTime? PasswordResetTokenExpirationDate { get; set; }

        [JilDirective(true)]
        public DateTime? CreatedUtc { get; set; }

        [JilDirective(true)]
        public DateTime? LastLoginUtc { get; set; }

        [JilDirective(true)]
        public ICollection<Credential> Credentials { get; set; }

        [JilDirective(true)]
        public bool IsAdmin { get; set; }

        [JilDirective(true)]
        public bool Confirmed
        {
            get { return !String.IsNullOrEmpty(EmailAddress); }
        }

        public bool HasPassword()
        {
            return Credentials.Any(c =>
                c.Type.StartsWith(CredentialTypes.Password.Prefix, StringComparison.OrdinalIgnoreCase));
        }
    }
}