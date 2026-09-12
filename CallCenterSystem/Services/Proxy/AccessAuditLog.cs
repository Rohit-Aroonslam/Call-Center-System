namespace CallCenterSystem.Services.Proxy
{
    // the logs or logging lives her. proxy records evry access decision as it delegates,
    // making the protection visible on screen instead of being hidden in code
    // hope it makes sense lads
    public class AccessAuditLog
    {
        private readonly List<AccessAttempt> _attempts = new();

        public event Action? Changed;

        // sorting to latest first
        public IReadOnlyList<AccessAttempt> Attempts =>
            _attempts.AsEnumerable().Reverse().ToList();

        public void Record(AccessAttempt attempt)
        {
            _attempts.Add(attempt);
            Changed?.Invoke();
        }

        public void clear()
        {
            _attempts.Clear();
            Changed?.Invoke();
        }
    }
}
