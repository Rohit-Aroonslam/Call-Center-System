using CallCenterSystem.Models;

namespace CallCenterSystem.Interfaces
{
    public interface ISessionContext
    {
        User? CurrentUser { get; }
        bool IsAuthenticated { get; }
        event Action? Changed;

        bool SignIn(string username, string password);
        void SignOut();
        IEnumerable<User> AvailableUsers { get; }
    }
}
