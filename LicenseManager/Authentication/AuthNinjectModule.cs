using LicenseManager.Authentication.Providers;
using Ninject.Modules;

namespace LicenseManager.Authentication
{
    public class AuthNinjectModule : NinjectModule
    {
        public override void Load()
        {
            foreach (var instance in Authenticator.GetAllAvailable())
            {
                Bind(typeof(Authenticator))
                    .ToConstant(instance)
                    .InSingletonScope();
            }
        }
    }
}