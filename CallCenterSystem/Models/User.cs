namespace CallCenterSystem.Models
{
    public class User
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public User( string username, string displayName, string password, UserRole role)
        {
            Username = username;
            DisplayName = displayName;
            Password = password;
            Role = role;
        }
    }
}
