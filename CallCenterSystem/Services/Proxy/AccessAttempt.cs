using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Proxy
{
    // One line of audit trail for every proxy allowed or denied attempt
    public record AccessAttempt
    (
        DateTime Timestamp,
        string Username,
        UserRole? Role,
        CallCenterOperation Operation,
        bool Allowed,
        string Detail
    );
}
