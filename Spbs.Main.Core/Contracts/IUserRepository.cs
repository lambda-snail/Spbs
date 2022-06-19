using Spbs.Main.Core.Models;

namespace Spbs.Main.Core.Contracts;

public interface IUserRepository
{
    Task<User> GetUserById(Guid userId);
    Task UpsertUserData(User user);
}