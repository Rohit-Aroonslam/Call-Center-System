using System;
using System.Collections.Generic;
using System.Linq;
using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;
using CallCenterSystem.Services.Composite;
using CallCenterSystem.Services.Iterator;

namespace CallCenterSystem.Services
{
    public class CallCenterAppService
    {
        private readonly CallLog _callLog = new();
        private int _nextId = 1;

        // Composite: the call center's org structure (departments, sub-teams,
        // and staff), built once the seed calls exist so it can report real
        // call counts per person straight away.
        private readonly Department _organization;

        // Raised whenever a call is added or a staff member is added, and
        // also pinged manually by ActiveCall while a live session changes
        // state, so every page showing the log or the org tree can refresh
        // itself instead of only updating on next navigation.
        public event Action? StateChanged;

        public CallCenterAppService()
        {
            // Seed the log so the page has something to show immediately
            AddCall("Rohit Aroonslam", "0712885274");
            AddCall("Amity Brown", "0824698738");
            AddCall("Marcellos Naidoo", "0835512290");
            AddCall("John Khumalo", "0719944531");

            _organization = BuildOrganization();
        }

        // Add a call. Validates first, throws with a message the page can display.
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

        // Iterator: walk the log into a display list without exposing internal storage.
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

        // Iterator search, the "manager wants to return a call" requirement.
        public Call? FindCall(int callId)
        {
            var iterator = (CallLogIterator)_callLog.CreateIterator();
            return iterator.Find(callId);
        }

        // Composite: expose the root of the org tree to the UI.
        public IOrgComponent GetOrganization() => _organization;

        // Every department and sub-department in the tree, for pickers
        // like the "Add a Staff Member" form.
        public IEnumerable<Department> GetDepartments() => _organization.AllDepartments();

        // Adds a new staff member (Leaf) into an existing department
        // (Composite) by name. Their call count is wired to the shared
        // CallLog immediately, the same way the seeded staff are, so a
        // newly added technician starts reflecting real calls right away.
        public void AddStaffMember(string departmentName, string name, string role)
        {
            var department = _organization.FindDepartment(departmentName)
                ?? throw new ArgumentException("Choose a valid department.");

            string nameError = ValidateName(name);
            if (nameError != "") throw new ArgumentException(nameError);

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be empty.");

            var trimmedName = name.Trim();
            department.Add(new StaffMember(trimmedName, role.Trim(), () => CallsFor(trimmedName)));

            NotifyStateChanged();
        }

        // Manually raised by ActiveCall.razor when a live session's state
        // changes (connect, hold, hang up, or each tick), so the Call Log
        // and Departments pages can refresh live instead of waiting for
        // the next navigation.
        public void NotifyCallsChanged() => NotifyStateChanged();

        private IEnumerable<Call> CallsFor(string callerName) =>
            GetAllCalls().Where(c => c.CallerName.Equals(callerName, StringComparison.OrdinalIgnoreCase));

        // Builds the department/staff tree. Each StaffMember is handed a
        // small delegate that pulls their calls straight out of the shared
        // CallLog (via the Iterator above), so the tree always reflects
        // whatever is currently logged rather than a frozen snapshot.
        private Department BuildOrganization()
        {
            var root = new Department("Student Support Call Center");

            var technicalSupport = new Department("Technical Support");
            technicalSupport.Add(new StaffMember("Rohit Aroonslam", "Technician", () => CallsFor("Rohit Aroonslam")));
            technicalSupport.Add(new StaffMember("Marcellos Naidoo", "Technician", () => CallsFor("Marcellos Naidoo")));

            // A sub-team nested inside Technical Support, showing the
            // Composite recursing through more than one level.
            var helpDesk = new Department("Help Desk");
            helpDesk.Add(new StaffMember("Bantu Khumalo", "Technician", () => CallsFor("Bantu Khumalo")));
            technicalSupport.Add(helpDesk);

            var studentServices = new Department("Student Services");
            studentServices.Add(new StaffMember("Amity Brown", "Student Liaison", () => CallsFor("Amity Brown")));

            root.Add(technicalSupport);
            root.Add(studentServices);

            return root;
        }

        // Validation
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