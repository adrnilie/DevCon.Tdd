using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevCon.Code.Business.Repositories;
using DevCon.Code.Business.Storage.Entities;

namespace DevCon.Code.Testing.Common.Fakes
{
    public class FakeDevConUserRepository : IDevConUserRepository
    {
        private static List<DevConUser> Users { get; } = new List<DevConUser>();

        public Task<DevConUser> FindUserAsync(Guid userId)
        {
            var user = Users.SingleOrDefault(x => x.Id == userId);

            return Task.FromResult(user);
        }

        public Task<bool> UserExistsAsync(string emailAddress)
        {
            var exists = Users.Any(x => x.EmailAddress == emailAddress);

            return Task.FromResult(exists);
        }

        public Task<DevConUser> CreateUserAsync(DevConUser user)
        {
            Users.Add(user);

            return Task.FromResult(user);
        }

        public Task<DevConUser> UpdateUserAsync(DevConUser user)
        {
            var dbUser = Users.SingleOrDefault(x => x.Id == user.Id);
            if (dbUser == null)
            {
                return Task.FromResult<DevConUser>(null);
            }

            UpdateUser(dbUser, user);

            return Task.FromResult(dbUser);
        }

        public void UpsertUsers(params DevConUser[] users)
        {
            Users.AddRange(users);
        }

        private static void UpdateUser(DevConUser dbUser, DevConUser user)
        {
            dbUser.EmailAddress = user.EmailAddress;
            dbUser.Active = user.Active;
            dbUser.EmailConfirmed = user.EmailConfirmed;
            dbUser.PasswordSalt = user.PasswordSalt;
            dbUser.PasswordHash = user.PasswordHash;
            dbUser.LastUpdated = DateTime.UtcNow;
        }
    }
}