// Services/Proxy/CallCenterServiceAdapter.cs
using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;
using CallCenterSystem.Services.Composite;
using CallCenterSystem.Services.State;

namespace CallCenterSystem.Services.Proxy
{
    // RealSubject.
    //
    // Ideally CallCenterAppService would implement ICallCenterService directly
    // and this class would not exist. It is here only because CallCenterAppService
    // is owned by another group member and must not be edited without approval.
    // Once approved, collapse this: put ": ICallCenterService" on
    // CallCenterAppService and delete this file.
    //
    // It also supplies ReturnCall(), which had no home before.
    public class CallCenterServiceAdapter : ICallCenterService
    {
        private readonly CallCenterAppService _app;

        public CallCenterServiceAdapter(CallCenterAppService app) => _app = app;

        public event Action? StateChanged
        {
            add => _app.StateChanged += value;
            remove => _app.StateChanged -= value;
        }

        public Call AddCall(string callerName, string phoneNumber) =>
            _app.AddCall(callerName, phoneNumber);

        public List<Call> GetAllCalls() => _app.GetAllCalls();

        public Call? FindCall(int callId) => _app.FindCall(callId);

        // Finds the logged call via the Iterator, then opens a State-pattern
        // CallSession on it. This is the hand-off point between Proxy, Iterator
        // and State.
        public CallSession ReturnCall(int callId)
        {
            var call = _app.FindCall(callId)
                ?? throw new ArgumentException($"No call with ID {callId} in the call log.");

            return new CallSession(call);
        }

        public IOrgComponent GetOrganization() => _app.GetOrganization();

        public IEnumerable<Department> GetDepartments() => _app.GetDepartments();

        public void AddStaffMember(string departmentName, string name, string role) =>
            _app.AddStaffMember(departmentName, name, role);

        public void NotifyCallsChanged() => _app.NotifyCallsChanged();
    }
}