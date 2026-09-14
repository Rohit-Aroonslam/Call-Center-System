// Services/Proxy/ICallCenterService.cs
using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;
using CallCenterSystem.Services.Composite;
using CallCenterSystem.Services.State;

namespace CallCenterSystem.Services.Proxy
{
    // Subject.
    // Both the RealSubject (CallCenterServiceAdapter) and the Proxy
    // (CallCenterServiceProxy) implement this. That identical signature is what
    // makes the Proxy pattern work: a page holding an ICallCenterService cannot
    // tell which one it has.
    public interface ICallCenterService
    {
        event Action? StateChanged;

        Call AddCall(string callerName, string phoneNumber);
        List<Call> GetAllCalls();
        Call? FindCall(int callId);

        // The privileged operation the brief names: the manager returning a call
        // from the virtual call log. It did not exist as a method before — the
        // UI built a CallSession inline — so it is introduced here.
        CallSession ReturnCall(int callId);

        IOrgComponent GetOrganization();
        IEnumerable<Department> GetDepartments();
        void AddStaffMember(string departmentName, string name, string role);
        void NotifyCallsChanged();
    }
}