using CallCenterSystem.Models;
using CallCenterSystem.Services.Composite;
using CallCenterSystem.Services.State;

namespace CallCenterSystem.Interfaces
{
    // Subject.
    // Both the RealSubject (CallCenterServiceAdapter) and the Proxy
    // (CallCenterServiceProxy) implement this. That identical signature is what
    // makes the Proxy pattern work: a page holding an ICallCenterService cannot
    // tell which one it is holding.
    public interface ICallCenterService
    {
        event Action? StateChanged;

        Call AddCall(string callerName, string phoneNumber);
        List<Call> GetAllCalls();
        Call? FindCall(int callId);

        // The privileged operation the brief names: the manager returning a call
        // from the virtual call log.
        CallSession ReturnCall(int callId);

        IOrgComponent GetOrganization();
        IEnumerable<Department> GetDepartments();
        void AddStaffMember(string departmentName, string name, string role);

        // Structural changes to the Composite tree. Guarded for the same reason
        // AddStaffMember is: they alter the organization, not just read it.
        void AddDepartment(string parentDepartmentName, string newDepartmentName);
        void RemoveNode(IOrgComponent? parent, IOrgComponent node);

        void NotifyCallsChanged();
    }
}