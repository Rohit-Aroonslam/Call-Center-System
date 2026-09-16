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
            var last = _attempts.LastOrDefault();

            if (last is not null &&
                last.Username == attempt.Username &&
                last.Role == attempt.Role &&
                last.Operation == attempt.Operation &&
                last.Allowed == attempt.Allowed &&
                last.Detail == attempt.Detail &&
                (attempt.Timestamp - last.Timestamp) < TimeSpan.FromSeconds(3))
            {
                return;
            }
            _attempts.Add(attempt);
            Changed?.Invoke();
        }

        public void Clear()
        {
            _attempts.Clear();
            Changed?.Invoke();
        }
    }
}
