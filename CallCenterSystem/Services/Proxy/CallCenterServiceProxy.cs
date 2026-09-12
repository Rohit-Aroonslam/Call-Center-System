// Services/Proxy/CallCenterServiceProxy.cs
using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;
using CallCenterSystem.Services.Composite;
using CallCenterSystem.Services.State;

namespace CallCenterSystem.Services.Proxy
{
    // Proxy.
    //
    // A protection proxy with a logging responsibility layered on. It implements
    // the same ICallCenterService interface as the RealSubject, so every caller
    // is unaware it is talking to a surrogate. Each method does exactly three
    // things: check the permission matrix for the current user's role, record
    // the attempt, then delegate. No business logic lives here — that stays in
    // the RealSubject.
    public class CallCenterServiceProxy : ICallCenterService
    {
        private readonly ICallCenterService _real;
        private readonly ISessionContext _session;
        private readonly AccessAuditLog _audit;

        public CallCenterServiceProxy(
            ICallCenterService real,
            ISessionContext session,
            AccessAuditLog audit)
        {
            _real = real;
            _session = session;
            _audit = audit;
        }

        public event Action? StateChanged
        {
            add => _real.StateChanged += value;
            remove => _real.StateChanged -= value;
        }

        // ---- access decision plumbing ---------------------------------------

        // Asks the permission matrix. Does NOT write to the audit trail, so it
        // can be called more than once per operation without producing noise.
        private bool IsAllowed(CallCenterOperation operation)
        {
            var user = _session.CurrentUser;
            return user is not null && Permissions.IsAllowed(user.Role, operation);
        }

        // Writes exactly one audit row. Callers decide when the decision is
        // final, so one user action produces one line.
        private void Record(CallCenterOperation operation, bool allowed, string detail = "")
        {
            var user = _session.CurrentUser;

            _audit.Record(new AccessAttempt(
                DateTime.Now,
                user?.DisplayName ?? "(not signed in)",
                user?.Role,
                operation,
                allowed,
                detail));
        }

        // The common case: one operation, one decision, allow or throw.
        private void Guard(CallCenterOperation operation, string detail = "")
        {
            bool allowed = IsAllowed(operation);
            Record(operation, allowed, detail);

            if (allowed) return;

            var user = _session.CurrentUser;
            string who = user is null ? "You are not signed in" : $"{user.Role}s are not";
            throw new AccessDeniedException(
                operation,
                user?.Role,
                $"Access denied. {who} permitted to perform '{operation}'.");
        }

        // ---- guarded operations ---------------------------------------------

        public Call AddCall(string callerName, string phoneNumber)
        {
            Guard(CallCenterOperation.LogCall, $"{callerName} / {phoneNumber}");
            return _real.AddCall(callerName, phoneNumber);
        }

        // Demonstrates that a proxy can narrow a result, not only refuse it.
        // Three outcomes, one audit row each:
        //   BrowseCallLog  -> the whole log
        //   ViewOwnCalls   -> only calls belonging to the signed-in user
        //   neither        -> nothing
        public List<Call> GetAllCalls()
        {
            if (IsAllowed(CallCenterOperation.BrowseCallLog))
            {
                Record(CallCenterOperation.BrowseCallLog, true, "full call log");
                return _real.GetAllCalls();
            }

            if (IsAllowed(CallCenterOperation.ViewOwnCalls))
            {
                var name = _session.CurrentUser!.DisplayName;

                var own = _real.GetAllCalls()
                               .Where(c => c.CallerName.Equals(name, StringComparison.OrdinalIgnoreCase))
                               .ToList();

                Record(CallCenterOperation.ViewOwnCalls, true, $"own calls only ({own.Count})");
                return own;
            }

            Record(CallCenterOperation.BrowseCallLog, false, "no read access to the call log");
            return new List<Call>();
        }

        public Call? FindCall(int callId)
        {
            Guard(CallCenterOperation.SearchCallLog, $"Call ID {callId}");
            return _real.FindCall(callId);
        }

        // The brief's privileged action: only the call center manager may return
        // a call from the virtual call log.
        public CallSession ReturnCall(int callId)
        {
            Guard(CallCenterOperation.ReturnCall, $"Call ID {callId}");
            return _real.ReturnCall(callId);
        }

        public IOrgComponent GetOrganization()
        {
            Guard(CallCenterOperation.ViewOrganization);
            return _real.GetOrganization();
        }

        public IEnumerable<Department> GetDepartments()
        {
            Guard(CallCenterOperation.ViewOrganization);
            return _real.GetDepartments();
        }

        public void AddStaffMember(string departmentName, string name, string role)
        {
            Guard(CallCenterOperation.AddStaffMember, $"{name} ({role}) → {departmentName}");
            _real.AddStaffMember(departmentName, name, role);
        }

        // Unguarded: a UI refresh notification carries no privileged data.
        public void NotifyCallsChanged() => _real.NotifyCallsChanged();
    }
}