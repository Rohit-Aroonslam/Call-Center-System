# Call Center Request Handling Simulation

## Project Overview

This project is a Call Center Request Handling application developed for the ONT412
assignment. It is a Blazor Server (.NET 8) web app that simulates how a student support
call center manages calls between students and technicians.

- Students and technicians can make, hold, and drop calls.
- A manager can browse and search the virtual call log to return a call.
- Staff are organised into departments and teams, with totals that roll up automatically
  regardless of how deeply the structure is nested.
- Access to sensitive actions, such as adding or removing staff, is controlled by role and
  logged, so who did what is always recoverable.

The project demonstrates the Iterator, State, Composite, and Proxy design patterns in C#,
covering call log traversal and search, call state management, hierarchical staff grouping,
and controlled, audited access to the system.

---

## Getting Started

**Prerequisites:** .NET 8 SDK and Visual Studio 2022 (or `dotnet` on the command line).

1. Open `CallCenterSystem.sln` in Visual Studio, or open the `CallCenterSystem` folder in a
   terminal.
2. Restore and run:
   - Visual Studio: press F5 (or Ctrl+F5 to run without debugging).
   - Command line: `dotnet run` from inside the `CallCenterSystem` project folder.
3. The app opens in your browser automatically, or visit the local address shown in the
   terminal (for example `https://localhost:5001`).
4. Some pages, such as Departments, require signing in first. Demo accounts for each role
   (Student, Technician, Manager) are listed on the Sign In page.

---

## Pages

| Route | Page | What it does |
|---|---|---|
| `/` | Home | Landing page with a quick overview and live counts pulled from the call log. |
| `/call-log` | Call Log | Log a new call, search for a call by ID, and browse every call recorded so far. |
| `/active-call` | Active Call | Start a live call, or pick one up from the log, and move it between On Call, On Hold, and Ended. |
| `/departments` | Department Management | View, search, and manage staff across departments; expand a staff member to see their full call history. |
| `/call-access` | Call Access | Demonstrates the Proxy directly: try different operations as different roles and see the access log update live. |
| `/login` | Sign In | Switch between demo accounts to see how role changes what the rest of the app allows. |

---

## Group Members

| Name | Responsibility |
|---|---|
| Rohit | Iterator Design Pattern |
| Marcellos | State Design Pattern |
| Amity | Composite Design Pattern |
| Bantu | Proxy Design Pattern |
| Jodi | Documentation |

---

## Technologies Used

- C#
- .NET 8
- Visual Studio 2022
- Blazor Server

---

## Design Patterns Implemented

| Pattern | Purpose | Key files |
|---|---|---|
| **Iterator** | Traverses the call log without exposing its internal storage, and supports searching for a specific call so a manager can return it. | `Services/Iterator/CallLog.cs`, `CallLogIterator.cs`, `Interfaces/ICallIterator.cs`, `ICallCollection.cs` |
| **State** | Manages a call's lifecycle: On Call, On Hold, and Ended, including talk time, hold time, and the final saved duration. | `Services/State/CallSession.cs`, `OnCallState.cs`, `OnHoldState.cs`, `HungUpState.cs`, `Interfaces/ICallState.cs` |
| **Composite** | Represents departments and staff as one uniform tree: a department can hold staff, or further sub-departments, and every total (staff count, call count) rolls up automatically at any depth. | `Services/Composite/Department.cs`, `StaffMember.cs`, `Interfaces/IOrgComponent.cs` |
| **Proxy** | Controls and logs access to sensitive operations (viewing the organization, adding or removing staff), enforcing a per-role permission matrix before any request reaches the real service. | `Services/Proxy/CallCenterServiceProxy.cs`, `CallCenterServiceAdapter.cs`, `Permissions.cs`, `CallCenterOperation.cs`, `AccessAuditLog.cs`, `AccessDeniedException.cs`, `Interfaces/ICallCenterService.cs` |

---

## How the Patterns Work Together

The four patterns are not four separate demos bolted together. The Department Management
page routes its reads and its "Add Staff" action through `CallCenterServiceProxy`, the same
access control used on the Call Access page, so only a signed-in Manager can change the
organization, and every attempt is recorded in the same audit trail. A department's staff
list is built from the same `Call` records the Iterator walks and the State pattern updates
live, so expanding a staff member on Department Management shows their real call history,
not sample data.

---

## Known Limitations and Possible Next Steps

- `AddDepartment` and removing a node are not yet guarded operations in the Proxy's
  permission matrix (`CallCenterOperation` has no entry for them), so they call
  `CallCenterAppService` directly rather than through the Proxy. The Department Management
  page still hides these controls from non-Managers for a consistent experience, but the
  server side does not yet enforce it. Adding these to the matrix would close that gap.
- The signed-in user (`ISessionContext`) is a single shared value for the whole app, so
  signing in on one browser tab affects every tab. This is fine for a class demonstration
  but would need per-user sessions in a real deployment.
- All data is in memory and resets when the app restarts; there is no database.

---

## Project Structure

```text
CallCenterSystem
|
├── Components
│   ├── Layout
│   ├── Pages
│   └── ...
│
├── Interfaces
├── Models
├── Services
│   ├── Iterator
│   ├── State
│   ├── Composite
│   └── Proxy
└── Program.cs
```
