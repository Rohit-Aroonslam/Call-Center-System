namespace CallCenterSystem.Services.Proxy
{
    // these act a guard for the proxy, instead of string
    // so the audit trail can't drift out of sync
    public enum CallCenterOperation
    {
        LogCall,
        ViewOwnCalls,
        BrowseCallLog,
        SearchCallLog,
        ReturnCall,
        ViewOrganization,
        AddStaffMember
    }
}
