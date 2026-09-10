using System;
using System.Collections.Generic;
using CallCenterSystem.Models;

namespace CallCenterSystem.Interfaces
{
    // Component
    // Shared interface for a single staff member (Leaf) and a whole
    // department/team (Composite). Because both implement the same
    // interface, CallCenterAppService and the UI can ask either one for
    // its staff count, its total calls handled, or the actual calls
    // underneath it, without needing to know whether they're looking at
    // one person or an entire branch of the call center.
    public interface IOrgComponent
    {
        string Name { get; }

        // Human-readable kind: "Department" for a Composite, or the
        // person's job title (e.g. "Technician") for a Leaf.
        string Role { get; }

        // How many people sit underneath this node (always 1 for a Leaf).
        int GetStaffCount();

        // The actual Call records handled by this node and everything
        // beneath it. For a Leaf this is just that person's own calls;
        // for a Composite it's every child's calls combined, computed
        // recursively. This is what lets the UI drill from a department,
        // down to a staff member, down to the calls they've handled.
        IEnumerable<Call> GetCalls();

        // Convenience total, equivalent to GetCalls().Count().
        int GetTotalCalls();

        // Flattens this node and all of its descendants into a
        // depth-ordered list, so simpler UI (or a report) can render the
        // whole tree with a single loop instead of recursive markup.
        IEnumerable<OrgNode> Flatten(int depth = 0);
    }

    // One row of the flattened org tree. Carries the underlying node
    // (Source) too, not just display fields, so a flat UI loop can still
    // reach GetCalls() for a staff member without any recursion.
    public record OrgNode(int Depth, string Name, string Role, int StaffCount, int TotalCalls, IOrgComponent Source);
}