using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Proxy
{
    public class SessionContext : ISessionContext
    {
        private readonly List<User> _users = new()
        {
            new User("amity",     "Amity Brown",      "manager",    UserRole.Manager),
            new User("rohit",     "Rohit Aroonslam",  "tech",       UserRole.Technician),
            new User("marcellos", "Marcellos Von Buchenroder", "tech",       UserRole.Technician),
            new User("bantu",     "Bantu-Bethu Beya",    "student",    UserRole.Student)
        };

        public User? CurrentUser { get; private set; }

        public bool IsAuthenticated => CurrentUser is not null;

        public IEnumerable<User> AvailableUsers => _users;

        public event Action? Changed;

        public bool SignIn(string username, string password)
        {
            var match = _users.FirstOrDefault(u =>
                u.Username.Equals(username?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (match is null) return false;

            CurrentUser = match;
            Changed?.Invoke();
            return true;            
        }

        public void SignOut()
        {
            CurrentUser = null;
            Changed?.Invoke();
        }
    }
}
