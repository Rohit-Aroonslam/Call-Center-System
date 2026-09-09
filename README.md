# Call Center Request Handling Simulation

## 📌 Project Overview
This project is a Call Center Request Handling Application developed for the ONT412 Assignment.

The system simulates how a student call center manages calls between students and technicians:
- Students and technicians can make, hold, and drop calls
- The manager can browse and search the virtual call log to return a call
- Calls can be grouped and controlled through structured, secure access

The project demonstrates the implementation of the **Iterator, State, Composite, and Proxy design patterns** using C# and .NET, showcasing call log traversal, call state management, hierarchical grouping, and controlled access.

---

## 👥 Group Members

| Name      | Responsibility |
|-----------|----------------|
| Rohit     | Iterator Design Pattern |
| Marcellos | State Design Pattern |
| Amity     | Composite Design Pattern |
| Bantu     | Proxy Design Pattern |
| Jodi      | Documentation |

---

## 🛠️ Technologies Used

- C#
- .NET 8
- Visual Studio 2022
- Blazor

---

## 🎯 Design Pattern Implemented

| Design&nbsp;Pattern | Purpose |
|----------------|---------|
| **Iterator** | Allows traversal through the call log without exposing its internal structure, and supports searching for a specific call to return. |
| **State** | Manages call states such as *On Call*, *On Hold*, and *Ended*. |
| **Proxy** | Controls access to call logs or restricted actions (e.g., manager-only operations). |
| **Composite** | Represents hierarchical structures (e.g., grouping multiple calls or organizing staff under departments). |

---

## 📂 Project Structure

```text
CallCenterSystem
│
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
