using System;
using System.Net.Mail;

namespace LicenseManager.Configuration
{
    public interface IAppConfiguration
    {
        /// <summary>
        /// True if site diagnostics should be enabled and viewable
        /// </summary>
        bool DiagnosticsEnabled { get; set; }

        /// <summary>
        /// Gets the protocol-independent site root
        /// </summary>
        string SiteRoot { get; set; }

        /// <summary>
        /// Gets a setting indicating if SSL is required for all operations once logged in.
        /// </summary>
        bool RequireSSL { get; set; }

        /// <summary>
        /// Gets the port used for SSL
        /// </summary>
        int SSLPort { get; set; }

        /// <summary>
        /// Gets a boolean indicating if the site is in read only mode
        /// </summary>
        bool ReadOnlyMode { get; set; }

        /// <summary>
        /// Gets the SQL Connection string used to connect to the database
        /// </summary>
        string SqlConnectionString { get; set; }

        /// <summary>
        /// Gets the local directory in which to store files.
        /// </summary>
        string FileStorageDirectory { get; set; }

        /// <summary>
        /// Gets the support email address
        /// </summary>
        MailAddress SupportEmail { get; set; }

        /// <summary>
        /// Gets the URI of the SMTP host to use. Or null if SMTP is not being used. Use <see cref="Configuration.SmtpUri"/> to parse it
        /// </summary>
        Uri SmtpUri { get; set; }
    }
}