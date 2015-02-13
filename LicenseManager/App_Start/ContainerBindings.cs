using System;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AnglicanGeek.MarkdownMailer;
using Elmah;
using LicenseManager.Configuration;
using LicenseManager.Infrastructure;
using LicenseManager.Models.ViewModels;
using LicenseManager.Services;
using LicenseManager.Services.Interfaces;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

namespace LicenseManager
{
    public class ContainerBindings : NinjectModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:CyclomaticComplexity", Justification = "This code is more maintainable in the same function.")]
        public override void Load()
        {
            var configuration = new ConfigurationService();
            Bind<ConfigurationService>().ToMethod(context => configuration);
            Bind<IAppConfiguration>().ToMethod(context => configuration.Current);
            Bind<IConfigurationSource>().ToMethod(context => configuration);

            Bind<AuditingService>().ToConstant(AuditingService.None);

            Bind<ErrorLog>()
                .ToMethod(_ => new SqlErrorLog(configuration.Current.SqlConnectionString))
                .InSingletonScope();

            Bind<ICacheService>().To<HttpContextCacheService>().InRequestScope();
            Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            Bind<IProductService>().To<ProductService>().InRequestScope();
            Bind<IProductVersionService>().To<ProductVersionService>().InRequestScope();
            Bind<IUserService>().To<UserService>().InRequestScope();

            Bind<IControllerFactory>()
                .To<CustomControllerFactory>()
                .InRequestScope();

            var mailSenderThunk = new Lazy<IMailSender>(
                () =>
                {
                    var settings = Kernel.Get<ConfigurationService>();
                    if (settings.Current.SmtpUri != null && settings.Current.SmtpUri.IsAbsoluteUri)
                    {
                        var smtpUri = new SmtpUri(settings.Current.SmtpUri);

                        var mailSenderConfiguration = new MailSenderConfiguration
                        {
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Host = smtpUri.Host,
                            Port = smtpUri.Port,
                            EnableSsl = smtpUri.Secure
                        };

                        if (!String.IsNullOrWhiteSpace(smtpUri.UserName))
                        {
                            mailSenderConfiguration.UseDefaultCredentials = false;
                            mailSenderConfiguration.Credentials = new NetworkCredential(
                                smtpUri.UserName,
                                smtpUri.Password);
                        }

                        return new MailSender(mailSenderConfiguration);
                    }
                    else
                    {
                        var mailSenderConfiguration = new MailSenderConfiguration
                        {
                            DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                            PickupDirectoryLocation = HostingEnvironment.MapPath("~/App_Data/Mail")
                        };

                        return new MailSender(mailSenderConfiguration);
                    }
                });

            Bind<IMailSender>().ToMethod(context => mailSenderThunk.Value);
            Bind<IMessageService>().To<MessageService>();
            Bind<IPrincipal>().ToMethod(context => HttpContext.Current.User);

            // ViewModel bindings
            Bind<INewLicenseViewModel>().To<CustomerSelectionViewModel>();
            Bind<INewLicenseViewModel>().To<ProductNameSelectionViewModel>();
        }
    }
}