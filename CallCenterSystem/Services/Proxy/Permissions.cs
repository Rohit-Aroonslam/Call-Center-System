using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Proxy
{
    // The proxy consults this instead of hard coded role check evry method.
    public static class Permissions
    {
        private static readonly Dictionary<UserRole, HashSet<CallCenterOperation>> Matrix = new()
        {
            [UserRole.Student] = new()
            {
                CallCenterOperation.LogCall,
                CallCenterOperation.ViewOwnCalls
            },
            [UserRole.Technician] = new()
            {
                CallCenterOperation.LogCall,
                CallCenterOperation.ViewOwnCalls,
                CallCenterOperation.BrowseCallLog,
                CallCenterOperation.SearchCallLog,
                CallCenterOperation.ViewOrganization
            },
            [UserRole.Manager] = new()
            {
                CallCenterOperation.LogCall,
                CallCenterOperation.ViewOwnCalls,
                CallCenterOperation.BrowseCallLog,
                CallCenterOperation.SearchCallLog,
                CallCenterOperation.ViewOrganization,
                CallCenterOperation.AddStaffMember,
                CallCenterOperation.ReturnCall
            }
        };

        public static bool IsAllowed(UserRole role, CallCenterOperation operation) =>
            Matrix.TryGetValue(role, out var allowed) && allowed.Contains(operation);

        public static IEnumerable<CallCenterOperation> AllOperations() =>
            Enum.GetValues<CallCenterOperation>();

        public static IEnumerable<UserRole> AllRoles() =>
            Enum.GetValues<UserRole>();
    }
}
