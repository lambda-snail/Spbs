namespace Spbs.Main.Core.Contracts;

public interface ILoggedInUserService
{
    Task<Guid> GetLoggedInUserId();
}