using System;
using System.Collections.Generic;
using System.Linq;
using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Composite
{
    // Leaf
    // A single technician or student-facing staff member. A StaffMember has
    // no children, so its GetStaffCount()/GetCalls() simply return its own
    // numbers. This is what lets Department treat a lone staff member and
    // an entire sub-department exactly the same way.
    public class StaffMember : IOrgComponent
    {
        public string Name { get; }
        public string Role { get; } // e.g. "Technician", "Student Liaison"

        // Injected rather than reading CallLog directly, so this Leaf stays
        // decoupled from the Iterator/Call classes; it only knows how to
        // ask "which calls are mine?" via whatever the app service hands
        // it (see CallCenterAppService.BuildOrganization).
        private readonly Func<IEnumerable<Call>> _callsProvider;

        public StaffMember(string name, string role, Func<IEnumerable<Call>>? callsProvider = null)
        {
            Name = name;
            Role = role;
            _callsProvider = callsProvider ?? (() => Enumerable.Empty<Call>());
        }

        public int GetStaffCount() => 1;

        public IEnumerable<Call> GetCalls() => _callsProvider();

        public int GetTotalCalls() => GetCalls().Count();

        public IEnumerable<OrgNode> Flatten(int depth = 0)
        {
            yield return new OrgNode(depth, Name, Role, GetStaffCount(), GetTotalCalls(), this);
        }
    }
}