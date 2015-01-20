using System.Net;
using System.Web.Http;
using LicenseManager.Models;
using LicenseManager.Services.Interfaces;

namespace LicenseManager.Controllers.Api
{
    public class UsersController : ApiController
    {
        #region DI

        public IUserService UserService { get; protected set; }

        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        #endregion

        public User Post(User user)
        {
            return new User();
        }

        public User Put(User user)
        {
            return new User();
        }

        public void Delete(int id)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            UserService.DeleteUser(id);
        }
    }
}
