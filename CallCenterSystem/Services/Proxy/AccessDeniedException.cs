using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Proxy
{
    // thrown by the proxy when current user's role does not allow requested operation.
    // rather than returning null and the refusal will be displayed on screen instead
    // of unhandled exceptions and ui fails.
    public class AccessDeniedException : Exception
    {
        public CallCenterOperation Operation { get; }
        public UserRole? Role { get; }

        public AccessDeniedException(CallCenterOperation operation, UserRole? role, string message):base (message)
        {
            Operation = operation;
            Role = role;
        }
    }
}
