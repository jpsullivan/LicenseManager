using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using LicenseManager.Configuration;
using LicenseManager.Models;
using LicenseManager.Services.Interfaces;
using StackExchange.Profiling.Helpers.Dapper;

namespace LicenseManager.Services
{
    public class UserService : IUserService
    {
        #region IoC

        public IAppConfiguration Config { get; protected set; }
        public AuditingService Auditing { get; protected set; }

        protected UserService() { }

        public UserService(
            IAppConfiguration config,
            AuditingService auditing)
            : this()
        {
            Config = config;
            Auditing = auditing;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(int userId)
        {
            const string sql = "select * from Users where Id = @userId";
            var user = Current.DB.Query<User>(sql, new { userId }).FirstOrDefault();
            if (user == null) return null;

            user.Credentials = GetUserCredentials(userId);
            user.Roles = GetUserRoles(userId);

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUser(string email)
        {
            const string sql = "select * from Users where EmailAddress = @email";

            var user = Current.DB.Query<User>(sql, new { email }).FirstOrDefault();
            if (user == null) return null;

            user.Credentials = GetUserCredentials(user.Id);
            user.Roles = GetUserRoles(user.Id);

            return user;
        }

        /// <summary>
        /// Fetches a user record that matches either the username or the email address.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUser(string username, string email)
        {
            const string sql = "select * from Users where Username = @username or EmailAddress = @email";

            var user = Current.DB.Query<User>(sql, new { username, email }).FirstOrDefault();
            if (user == null) return null;

            user.Credentials = GetUserCredentials(user.Id);
            user.Roles = GetUserRoles(user.Id);

            return user;
        }

        /// <summary>
        /// Fetches all users within the system.
        /// </summary>
        /// <returns>A collection of all <see cref="User"/>s</returns>
        public IEnumerable<User> GetUsers()
        {
            var users = Current.DB.Users.All();

            var allUsers = users as IList<User> ?? users.ToList();
            foreach (var user in allUsers)
            {
                user.Credentials = GetUserCredentials(user.Id);
                user.Roles = GetUserRoles(user.Id);
            }

            return allUsers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            var success = Current.DB.Users.Insert(new
            {
                user.Username,
                user.EmailAddress,
                user.FullName,
                user.IsAdmin,
                user.EmailAllowed,
                user.PasswordResetToken,
                user.PasswordResetTokenExpirationDate,
                user.CreatedUtc,
                user.LastLoginUtc
            });

            if (success == null)
            {
                // insert failed. todo: throw message here
                return user;
            }

            user.Id = success.Value;

            // now add in the credentials
            foreach (var cred in user.Credentials)
            {
                Current.DB.Credentials.Insert(new
                {
                    UserId = user.Id,
                    Type = cred.Type,
                    Value = cred.Value,
                    Ident = cred.Ident
                });
            }

            return user;
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to update.</param>
        public void UpdateUser(User user)
        {
            var original = Current.DB.Users.Get(user.Id);

            // track which fields change on the object
            var s = Snapshotter.Start(original);
            original.Username = user.Username;
            original.EmailAddress = user.EmailAddress;
            original.FullName = user.FullName;
            original.Messages = user.Messages;
            original.Roles = user.Roles;
            original.EmailAllowed = user.EmailAllowed;
            original.PasswordResetToken = user.PasswordResetToken;
            original.PasswordResetTokenExpirationDate = user.PasswordResetTokenExpirationDate;
            original.Credentials = user.Credentials;
            original.IsAdmin = user.IsAdmin;

            var diff = s.Diff();
            if (diff.ParameterNames.Any())
            {
                Current.DB.Users.Update(user.Id, diff);
            }
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the <see cref="User"/> to delete.</param>
        public void DeleteUser(int id)
        {
            // first remove any credentials for this user
            DeleteUserCredentials(id);

            const string sql = "delete from Users where ID = @id";
            Current.DB.Execute(sql, new {id});
        }

        /// <summary>
        /// Fetches any credentials for the specified <see cref="User"/>.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ICollection<Credential> GetUserCredentials(int userId)
        {
            const string sql = "select * from Credentials where UserId = @userId";
            return Current.DB.Query<Credential>(sql, new { userId }).ToList();
        }

        /// <summary>
        /// Removes any credentials tied to the specified user id.
        /// </summary>
        /// <param name="userId">The user ID</param>
        public void DeleteUserCredentials(int userId)
        {
            const string sql = "delete from Credentials where UserId = @userId";
            Current.DB.Execute(sql, new {userId});
        }

        /// <summary>
        /// Fetches the roles that the supplied <see cref="User"/> is assigned to.
        /// </summary>
        /// <param name="userId">The user id to use when searching for roles.</param>
        /// <returns></returns>
        public ICollection<Role> GetUserRoles(int userId)
        {
            var builder = new SqlBuilder();
            SqlBuilder.Template tmpl = builder.AddTemplate(@"
                SELECT r.Id, r.Name 
                FROM UserRoles ur 
                /**join**/
                /**where**/"
                );

            builder.Join("Roles AS r ON r.Id = ur.RoleId");
            builder.Where("ur.UserId = @userId");

            return Current.DB.Query<Role>(tmpl.RawSql, new { userId }).ToList();
        }
    }
}