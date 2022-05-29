namespace Spbs.Main.WebUi.Contracts;

public interface ILoggedInUserService
{
    Task<Guid> GetLoggedInUserId();
}