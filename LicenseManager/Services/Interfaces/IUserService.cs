using System.Collections.Generic;
using LicenseManager.Models;

namespace LicenseManager.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Fetches a user record that matches either the username.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User GetUser(int userId);

        /// <summary>
        /// Fetches a user record that matches either the email address.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User GetUser(string email);

        /// <summary>
        /// Fetches a user record that matches either the username or the email address.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        User GetUser(string username, string email);

        /// <summary>
        /// Fetches all users within the system.
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetUsers();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User CreateUser(User user);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to update.</param>
        void UpdateUser(User user);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the <see cref="User"/> to delete.</param>
        void DeleteUser(int id);

        /// <summary>
        /// Fetches any credentials for the specified <see cref="User"/>.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ICollection<Credential> GetUserCredentials(int userId);

        /// <summary>
        /// Removes any credentials tied to the specified user id.
        /// </summary>
        /// <param name="userId">The user ID</param>
        void DeleteUserCredentials(int userId);

        /// <summary>
        /// Fetches the roles that the supplied <see cref="User"/> is assigned to.
        /// </summary>
        /// <param name="userId">The user id to use when searching for roles.</param>
        /// <returns></returns>
        ICollection<Role> GetUserRoles(int userId);
    }
}