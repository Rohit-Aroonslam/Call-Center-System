using System;
using System.Collections.Generic;
using System.Linq;
using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Composite
{
    // Composite
    // A department (or team) that can hold both individual staff members
    // (Leaf) and further sub-departments (Composite), since both implement
    // IOrgComponent. Every operation just delegates to its children and
    // sums/flattens the results, so adding another layer of structure (a
    // team inside a department inside the call center) never requires
    // changing this class or the code that calls it.
    public class Department : IOrgComponent
    {
        public string Name { get; }
        public string Role => "Department";

        private readonly List<IOrgComponent> _children = new();

        public Department(string name) => Name = name;

        // Add/Remove are the Composite-specific management operations.
        // A plain StaffMember Leaf doesn't expose these, only Department does.
        public void Add(IOrgComponent component) => _children.Add(component);
        public void Remove(IOrgComponent component) => _children.Remove(component);

        public IReadOnlyList<IOrgComponent> Children => _children;

        public int GetStaffCount() => _children.Sum(c => c.GetStaffCount());

        public IEnumerable<Call> GetCalls() => _children.SelectMany(c => c.GetCalls());

        public int GetTotalCalls() => GetCalls().Count();

        public IEnumerable<OrgNode> Flatten(int depth = 0)
        {
            yield return new OrgNode(depth, Name, Role, GetStaffCount(), GetTotalCalls(), this);

            foreach (var child in _children)
            {
                foreach (var node in child.Flatten(depth + 1))
                {
                    yield return node;
                }
            }
        }

        // Finds a department by name, searching this node and every
        // sub-department beneath it (depth-first). Used by the "Add a
        // Staff Member" form so a new hire can be dropped into any
        // department in the tree, not just the top level.
        public Department? FindDepartment(string name)
        {
            if (Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return this;

            foreach (var sub in _children.OfType<Department>())
            {
                var found = sub.FindDepartment(name);
                if (found != null)
                    return found;
            }

            return null;
        }

        // This department plus every sub-department beneath it, flattened
        // into one list, so the UI can offer a dropdown of every place a
        // new staff member could be added.
        public IEnumerable<Department> AllDepartments()
        {
            yield return this;

            foreach (var sub in _children.OfType<Department>())
            {
                foreach (var d in sub.AllDepartments())
                {
                    yield return d;
                }
            }
        }
    }
}