using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;
using CallCenterSystem.Services.Iterator;

namespace CallCenterSystem.Services
{
    public class CallCenterAppService
    {
        private readonly CallLog _callLog = new();
        private int _nextId = 1;

        public event Action? StateChanged;

        public CallCenterAppService()
        {
            // Seed the log so the page has something to show immediately
            AddCall("Rohit Aroonslam", "0712885274");
            AddCall("Amity Brown", "0824698738");
        }

        // ── Add a call — validates first, throws with a message the page can display ──
        public Call AddCall(string callerName, string phoneNumber)
        {
            string nameError = ValidateName(callerName);
            if (nameError != "") throw new ArgumentException(nameError);

            string phoneError = ValidatePhoneNumber(phoneNumber);
            if (phoneError != "") throw new ArgumentException(phoneError);

            var call = new Call(_nextId, callerName.Trim(), phoneNumber.Trim()) { Status = "Ended" };
            _callLog.AddCall(call);
            _nextId++;

            NotifyStateChanged();
            return call;
        }

        // ── Iterator — walk the log into a display list without exposing internal storage ──
        public List<Call> GetAllCalls()
        {
            var result = new List<Call>();
            ICallIterator iterator = _callLog.CreateIterator();

            for (iterator.First(); !iterator.IsDone(); iterator.Next())
            {
                var call = iterator.CurrentItem();
                if (call != null) result.Add(call);
            }

            return result;
        }

        // ── Iterator search — "manager wants to return a call" requirement ──
        public Call? FindCall(int callId)
        {
            var iterator = (CallLogIterator)_callLog.CreateIterator();
            return iterator.Find(callId);
        }

        // ── Validation ──────────────────────────────────────────────
        public static string ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Name cannot be empty.";

            foreach (char c in name)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                    return "Name may only contain letters, spaces, and hyphens.";
            }

            return "";
        }

        public static string ValidatePhoneNumber(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "Phone number cannot be empty.";

            string cleaned = phone.Trim().Replace(" ", "");
            if (cleaned.Length != 10 || !long.TryParse(cleaned, out _))
                return "Phone number must be exactly 10 digits, numbers only.";

            return "";
        }

        private void NotifyStateChanged() => StateChanged?.Invoke();
    }
}
